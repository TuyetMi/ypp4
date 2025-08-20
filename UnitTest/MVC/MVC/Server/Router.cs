using System.Net;
using System.Reflection;
using System.Text;
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

        public async Task<string> RouteAsync(HttpListenerRequest request)
        {
            var parts = request.Url.AbsolutePath.Trim('/').Split('/');
            if (parts.Length < 1) return "{\"error\":\"Not found\"}";

            string controllerName = parts[0] + "Controller";
            string actionName = parts.Length > 1 ? parts[1] : null;

            // Lấy controller type từ DI
            var controllerType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return new Type[0]; }
                })
                .FirstOrDefault(t => t.Name.Equals(controllerName, StringComparison.OrdinalIgnoreCase));

            if (controllerType == null) return "{\"error\":\"Controller not found\"}";

            var controller = _scope.Resolve(controllerType);
            if (controller == null) return "{\"error\":\"Controller not registered in DI\"}";

            MethodInfo? method = null;
            object[] parameters = Array.Empty<object>();

            if (!string.IsNullOrEmpty(actionName))
            {
                var methods = controllerType.GetMethods()
                    .Where(m => m.Name.Equals(actionName, StringComparison.OrdinalIgnoreCase));

                method = methods.FirstOrDefault(m =>
                {
                    var httpAttr = m.GetCustomAttribute<HttpMethodAttribute>();
                    return httpAttr == null || httpAttr.Method.Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase);
                });

                if (method != null)
                {
                    var paramInfos = method.GetParameters();
                    if (paramInfos.Length == 1)
                    {
                        if (request.HttpMethod == "POST" || request.HttpMethod == "PUT")
                        {
                            using var reader = new System.IO.StreamReader(request.InputStream);
                            var json = await reader.ReadToEndAsync();
                            var param = JsonSerializer.Deserialize(json, paramInfos[0].ParameterType);
                            parameters = new object[] { param! };
                        }
                        else if (parts.Length > 2)
                        {
                            var param = Convert.ChangeType(parts[2], paramInfos[0].ParameterType);
                            parameters = new object[] { param! };
                        }
                    }
                }
            }

            if (method == null) return "{\"error\":\"Action not found\"}";

            var result = method.Invoke(controller, parameters);

            if (result is Task task)
            {
                await task;
                var taskType = task.GetType();
                if (taskType.IsGenericType)
                {
                    var ret = taskType.GetProperty("Result")!.GetValue(task);
                    return JsonSerializer.Serialize(ret);
                }
                return "{}";
            }

            return JsonSerializer.Serialize(result);
        }
    }

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
