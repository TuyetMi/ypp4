using MVC.Controllers;

namespace MVC.Models
{
    public class Router
    {
        private readonly AccountController _accountController = new AccountController();

        public (string content, string contentType) Route(string path, string method)
        {
            if (path == "/home" && method == "GET")
                return (File.ReadAllText("Views/HomeView.html"), "text/html");

            if (path == "/lookup" && method == "GET")
                return (File.ReadAllText("Views/LookupView.html"), "text/html");

            if (path.StartsWith("/api/account/") && method == "GET")
            {
                int id = int.Parse(path.Split('/')[3]);
                return (_accountController.GetAccountInfoByIdJson(id), "application/json");
            }

            return ("<h1>404 Not Found</h1>", "text/html");
        }
    }
}
