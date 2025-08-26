using MVC.Controllers;
using MVC.Helpers;

namespace MVC.Models
{
    public class Router
    {

        private readonly DependencyInjectionConfig _di;

        public Router(DependencyInjectionConfig di)
        {
            _di = di;
        }
        public (string content, string contentType) Route(string path, string method)
        {
            using var scope = new DIScope(_di); // mỗi request 1 scope

            // Home page
            if (path == "/home" && method == "GET")
            {
                return (File.ReadAllText("Views/HomeView.html"), "text/html");
            }

            // Lookup page
            if (path == "/lookup" && method == "GET")
            {
                return (File.ReadAllText("Views/LookupView.html"), "text/html");
            }

            // API trả JSON
            if (path.StartsWith("/api/account/") && method == "GET")
            {
                // Lấy id từ URL: /api/account/{id}
                var parts = path.Split('/');
                if (parts.Length < 4)
                    return ("{}", "application/json");

                if (!int.TryParse(parts[3], out int id))
                    return ("{}", "application/json");

                // Lấy controller từ DI scope
                var accountController = scope.Resolve<AccountController>();

                // Lấy JSON từ controller (async => .Result cho demo)
                var json = accountController.GetAccountInfoByIdJson(id).Result;

                return (json, "application/json");
            }

            // 404 Not Found
            return ("<h1>404 Not Found</h1>", "text/html");
        }
    }
}

