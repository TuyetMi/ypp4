using MVC.Controllers;
using MVC.Helpers;
using MVC.Models;
using MVC.Repositories.AccountRepository;
using MVC.Services.AccountService;

namespace MVC.Router
{
    public class Router
    {
        private readonly Dictionary<string, Func<Task<object?>>> _routes = new();

        public void Register(string path, Func<Task<object?>> action)
        {
            _routes[path] = action;
        }

        public async Task<object?> HandleRequest(string path)
        {
            if (_routes.TryGetValue(path, out var action))
            {
                return await action();
            }
            return $"404 Not Found: {path}";
        }
    }

    public static class RouterConfig
    {
        public static Router Configure()
        {
            // Tạo DI config
            var di = AppDependencyInjectionConfig.CreateConfig();

            // Tạo scope (quan trọng để giữ Scoped services như DbConnection, Repository)
            var scope = new DIScope(di);

            // Resolve controller từ DI thay vì new
            var controller = scope.Resolve<AccountController>();

            var router = new Router();

            // Đăng ký các route
            router.Register("/account/all", async () => await controller.GetAll());
            router.Register("/account/info/all", async () => await controller.GetAllAccountInfo());
            router.Register("/account/create", async () =>
            {
                var acc = new Account { Id = 1, Name = "Alice" };
                return await controller.Create(acc);
            });
            router.Register("/account/get/1", async () => await controller.GetById(1));
            router.Register("/account/info/1", async () => await controller.GetAccountInfoById(1));

            return router;
        }
    }

}
