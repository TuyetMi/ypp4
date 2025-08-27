using System;
using System.Collections.Generic;
using System.IO;
using MVC.Controllers;
using MVC.Helpers;

namespace MVC.Server
{
    public class Router
    {
        private readonly DependencyInjectionConfig _di;

        private readonly Dictionary<(string path, string method), Func<DIScope, (string, string)>> _routes;

        public Router(DependencyInjectionConfig di)
        {
            _di = di;

            _routes = new Dictionary<(string, string), Func<DIScope, (string, string)>>
            {
                {
                    ("/home", "GET"),
                    scope => (File.ReadAllText(GetViewPath("HomeView.html")), "text/html")
                },
                {
                    ("/lookup", "GET"),
                    scope => (File.ReadAllText(GetViewPath("LookupView.html")), "text/html")
                }
            };
        }

        private string GetViewPath(string fileName)
        {
            var baseDir = AppContext.BaseDirectory;
            var projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"D:\\YPP4 GIT\\ypp4\\UnitTest\\MVC.View\\MVC\\MVC"));
            return Path.Combine(projectRoot, "Views", fileName);
        }

        public (string content, string contentType) Route(string path, string method)
        {
            using var scope = new DIScope(_di);

            // Check static route (home, lookup)
            if (_routes.TryGetValue((path, method), out var handler))
            {
                return handler(scope);
            }

            // Check dynamic API route
            if (path.StartsWith("/api/account/") && method == "GET")
            {
                var parts = path.Split('/');
                if (parts.Length < 4 || !int.TryParse(parts[3], out int id))
                    return ("{}", "application/json");

                var accountController = scope.Resolve<AccountController>();
                var json = accountController.GetAccountInfoByIdJson(id).Result;
                return (json, "application/json");
            }

            // Default 404
            return ("<h1>404 Not Found</h1>", "text/html");
        }
    }
}
