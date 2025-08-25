using System.Net;
using System.Reflection;
using System.Text.Json;
using MVC.Helpers;

namespace MVC.Server
{
    public class Router
    {
        private readonly DIScope _scope;

        public Router(DIScope scope)
        {
            _scope = scope;
        }

        /// <summary>
        /// Handle request -> find controller, action, bind parameters -> return JSON response
        /// </summary>
        public async Task<string> RouteAsync(HttpListenerRequest request)
        {
            // 1. Resolve controller + action from URL
            var (controllerType, actionName) = ResolveControllerAndAction(request.Url.AbsolutePath);
            if (controllerType == null) return Json(new { error = "Controller not found" });

            // 2. Resolve controller from DI container
            var controller = _scope.Resolve(controllerType);
            if (controller == null) return Json(new { error = "Controller not registered in DI" });

            // 3. Find matching action method by HttpMethod
            var method = FindActionMethod(controllerType, actionName, request.HttpMethod);
            if (method == null) return Json(new { error = "Action not found" });

            // 4. Bind parameters (from body or path)
            var parameters = await BindParametersAsync(method, request);

            // 5. Invoke method
            var result = method.Invoke(controller, parameters);

            // 6. Return JSON response
            return await ToJsonResultAsync(result);
        }

        /// <summary>
        /// Parse URL into Controller + Action
        /// </summary>
        private (Type?, string?) ResolveControllerAndAction(string path)
        {
            var parts = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);

            // Default root (/) → HomeController.Index
            if (parts.Length == 0)
            {
                return (FindController("HomeController"), "Index");
            }

            // If only /home → HomeController.Index
            if (parts.Length == 1)
            {
                string controllerName = parts[0] + "Controller";
                return (FindController(controllerName), "Index");
            }

            // If /controller/action
            string controllerNameFull = parts[0] + "Controller";
            string actionName = parts[1];
            return (FindController(controllerNameFull), actionName);
        }

        /// <summary>
        /// Helper to find controller by name
        /// </summary>
        private Type? FindController(string controllerName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return Array.Empty<Type>(); }
                })
                .FirstOrDefault(t => t.Name.Equals(controllerName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Find action method by name + HttpMethod (GET/POST/PUT/DELETE)
        /// </summary>
        private MethodInfo? FindActionMethod(Type controllerType, string? actionName, string httpMethod)
        {
            if (string.IsNullOrEmpty(actionName)) return null;

            return controllerType.GetMethods()
                .FirstOrDefault(m =>
                {
                    if (!m.Name.Equals(actionName, StringComparison.OrdinalIgnoreCase)) return false;
                    var httpAttr = m.GetCustomAttribute<HttpMethodAttribute>();
                    return httpAttr == null || httpAttr.Method.Equals(httpMethod, StringComparison.OrdinalIgnoreCase);
                });
        }

        /// <summary>
        /// Bind parameters for method (if any)
        /// </summary>
        private async Task<object[]> BindParametersAsync(MethodInfo method, HttpListenerRequest request)
        {
            var paramInfos = method.GetParameters();
            if (paramInfos.Length == 0) return Array.Empty<object>();

            var paramType = paramInfos[0].ParameterType;

            // If POST/PUT => read JSON body
            if (request.HttpMethod == "POST" || request.HttpMethod == "PUT")
            {
                using var reader = new StreamReader(request.InputStream);
                var json = await reader.ReadToEndAsync();
                var param = JsonSerializer.Deserialize(json, paramType);
                return new[] { param! };
            }

            // If GET/DELETE => take param from URL segment
            var parts = request.Url.AbsolutePath.Trim('/').Split('/');
            if (parts.Length > 2)
            {
                var param = Convert.ChangeType(parts[2], paramType);
                return new[] { param! };
            }

            return Array.Empty<object>();
        }

        /// <summary>
        /// Convert method result into JSON
        /// </summary>
        private async Task<string> ToJsonResultAsync(object? result)
        {
            if (result is Task task)
            {
                await task;
                var taskType = task.GetType();
                if (taskType.IsGenericType)
                {
                    var ret = taskType.GetProperty("Result")!.GetValue(task);
                    return Json(ret);
                }
                return "{}";
            }

            return Json(result);
        }

        /// <summary>
        /// JSON serialization helper
        /// </summary>
        private string Json(object? obj) => JsonSerializer.Serialize(obj);
    }

    // =========================
    // Attributes for Http Methods
    // =========================
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpMethodAttribute : Attribute
    {
        public string Method { get; }
        public HttpMethodAttribute(string method) => Method = method;
    }

    public class HttpGetAttribute : HttpMethodAttribute { public HttpGetAttribute() : base("GET") { } }
    public class HttpPostAttribute : HttpMethodAttribute { public HttpPostAttribute() : base("POST") { } }
    public class HttpPutAttribute : HttpMethodAttribute { public HttpPutAttribute() : base("PUT") { } }
    public class HttpDeleteAttribute : HttpMethodAttribute { public HttpDeleteAttribute() : base("DELETE") { } }
}
