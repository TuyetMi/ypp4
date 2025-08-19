

//using MVC.Models;
//using MVC.Services.AccountService;
//using System.Diagnostics.Metrics;
//using System.Net;
//using System.Text;

//namespace MVC.Controllers
//{
//    public class AccountController
//    {
//        private readonly IAccountService _service;
//        private readonly Router _router;

//        public AccountController(IAccountService service, Router router)
//        {
//            _service = service;
//            _router = router;
//            RegisterRoutes();
//        }

//        private void RegisterRoutes()
//        {
//            _router.RegisterRoute("GET /accounts", async (query) =>
//            {
//                var accounts = await _service.GetAllAsync();
//                return string.Join("\n", accounts.Select(a => $"{a.Id}: {a.FirstName} {a.LastName}"));
//            });

//            _router.RegisterRoute("GET /accounts/info", async (query) =>
//            {
//                var infos = await _service.GetAllAccountInfoAsync();
//                return string.Join("\n", infos.Select(i => $"{i.Id}: {i.FirstName} {i.LastName}"));
//            });

//            _router.RegisterRoute("GET /accounts/{id}", async (query) =>
//            {
//                if (!query.ContainsKey("id")) return "Missing id";
//                if (!int.TryParse(query["id"], out int id)) return "Invalid id";

//                var account = await _service.GetByIdAsync(id);
//                return account == null ? "Not found" : $"{account.Id}: {account.FirstName} {account.LastName}";
//            });

//            _router.RegisterRoute("GET /accounts/info/{id}", async (query) =>
//            {
//                if (!query.ContainsKey("id")) return "Missing id";
//                if (!int.TryParse(query["id"], out int id)) return "Invalid id";

//                var info = await _service.GetAccountInfoByIdAsync(id);
//                return info == null ? "Not found" : $"{info.Id}: {info.FirstName} {info.LastName}";
//            });

//            _router.RegisterRoute("POST /accounts", async (query) =>
//            {
//                var account = new Account
//                {
//                    FirstName = query.GetValueOrDefault("FirstName", ""),
//                    LastName = query.GetValueOrDefault("LastName", "")
//                };
//                int id = await _service.CreateAsync(account);
//                return $"Created account with id {id}";
//            });

//            _router.RegisterRoute("PUT /accounts/{id}", async (query) =>
//            {
//                if (!query.ContainsKey("id") || !int.TryParse(query["id"], out int id))
//                    return "Invalid id";

//                var account = new Account
//                {
//                    Id = id,
//                    FirstName = query.GetValueOrDefault("FirstName", ""),
//                    LastName = query.GetValueOrDefault("LastName", "")
//                };
//                int updated = await _service.UpdateAsync(account);
//                return updated > 0 ? "Updated" : "Not found";
//            });

//            _router.RegisterRoute("DELETE /accounts/{id}", async (query) =>
//            {
//                if (!query.ContainsKey("id") || !int.TryParse(query["id"], out int id))
//                    return "Invalid id";

//                int deleted = await _service.DeleteAsync(id);
//                return deleted > 0 ? "Deleted" : "Not found";
//            });
//        }

//        public Task<string> HandleRequest(string method, string path, Dictionary<string, string> query)
//        {
//            return _router.HandleRequest(method, path, query);
//        }
//    }
//}

