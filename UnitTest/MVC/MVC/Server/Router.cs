
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace MVC.Router
{
    public class Router
    {
        private readonly IServiceProvider _scope;

        public Router(IServiceProvider scope)
        {
            _scope = scope;
        }

        public async Task<string> RouteAsync(HttpListenerRequest request)
        {
            // /controller/action/params
            var parts = request.Url.AbsolutePath.Trim('/').Split('/');
            if (parts.Length < 1) return "{\"error\":\"Not found\"}";

            string controllerName = parts[0] + "Controller"; // account -> AccountController
            string actionName = parts.Length > 1 ? parts[1] : null;

            // Lấy controller type
            var controllerType = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name.Equals(controllerName, StringComparison.OrdinalIgnoreCase));

            if (controllerType == null) return "{\"error\":\"Controller not found\"}";

            // Resolve controller từ DI
            var controller = _scope.GetService(controllerType);
            if (controller == null) return "{\"error\":\"Controller not registered in DI\"}";

            MethodInfo? method = null;
            object[] parameters = Array.Empty<object>();

            // Chọn action dựa trên HTTP method
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
                        // POST/PUT body
                        if (request.HttpMethod == "POST" || request.HttpMethod == "PUT")
                        {
                            using var reader = new System.IO.StreamReader(request.InputStream);
                            var json = await reader.ReadToEndAsync();
                            var param = JsonSerializer.Deserialize(json, paramInfos[0].ParameterType);
                            parameters = new object[] { param! };
                        }
                        // GET URL param
                        else if (paramInfos.Length == 1 && parts.Length > 2)
                        {
                            var param = Convert.ChangeType(parts[2], paramInfos[0].ParameterType);
                            parameters = new object[] { param! };
                        }
                    }
                }
            }

            if (method == null) return "{\"error\":\"Action not found\"}";

            // Invoke action
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
    // Attribute hỗ trợ phân biệt HTTP method
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
