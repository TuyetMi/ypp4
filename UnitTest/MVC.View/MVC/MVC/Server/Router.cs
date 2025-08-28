
using MVC.Controllers;
using MVC.Helpers;

namespace MVC.Server
{
    public class Router
    {
        private readonly DependencyInjectionConfig _di;
        private readonly List<RouteDefinition> _routes = new();

        public Router(DependencyInjectionConfig di)
        {
            _di = di;
            RegisterViewRoutes();
            RegisterApiRoutes();
        }

        private void RegisterViewRoutes()
        {
            AddRoute("GET", "/home", scope => (LoadView("HomeView.html"), "text/html"));
            AddRoute("GET", "/lookup", scope => (LoadView("LookupView.html"), "text/html"));
        }

        private void RegisterApiRoutes()
        {
            // API routes
            AddRoute("GET", "/api/account/{id}", ctx =>
            {
                var accountController = ctx.Scope.Resolve<AccountController>();
                var id = ctx.RouteParams["id"].AsInt();
                var json = accountController.GetAccountInfoById(id).Result;
                return (json, "application/json");
            });

            AddRoute("GET", "/api/workspace/personal/{accountId}", ctx =>
            {
                var workspaceController = ctx.Scope.Resolve<WorkspaceController>();
                var accountId = ctx.RouteParams["accountId"].AsInt();
                var json = workspaceController.GetPersonalWorkspace(accountId).Result;
                return (json, "application/json");
            });

            AddRoute("GET", "/api/workspace/{id}", ctx =>
            {
                var workspaceController = ctx.Scope.Resolve<WorkspaceController>();
                var id = ctx.RouteParams["id"].AsInt();
                var json = workspaceController.GetWorkSpaceInfoById(id).Result;
                return (json, "application/json");
            });
        }

        private void AddRoute(string method, string template, Func<RouteContext, (string, string)> handler)
        {
            _routes.Add(new RouteDefinition(method, template, handler));
        }

        private string LoadView(string fileName)
        {
            var baseDir = AppContext.BaseDirectory;
            var projectRoot = Path.GetFullPath(Path.Combine(baseDir, @"D:\\YPP4 GIT\\ypp4\\UnitTest\\MVC.View\\MVC\\MVC"));
            return File.ReadAllText(Path.Combine(projectRoot, "Views", fileName));
        }

        public (string content, string contentType) Route(string path, string method)
        {
            foreach (var route in _routes)
            {
                var match = route.Match(method, path);
                if (match != null)
                {
                    using var scope = new DIScope(_di);
                    var ctx = new RouteContext(scope, match);
                    return route.Handler(ctx);
                }
            }

            return ("<h1>404 Not Found</h1>", "text/html");
        }
    }


    public class RouteContext
    {
        public DIScope Scope { get; }
        public Dictionary<string, string> RouteParams { get; }

        public RouteContext(DIScope scope, Dictionary<string, string> routeParams)
        {
            Scope = scope;
            RouteParams = routeParams;
        }
    }

    public static class RouteParamExtensions
    {
        public static int AsInt(this string value) => int.TryParse(value, out var result) ? result : 0;
    }
}
