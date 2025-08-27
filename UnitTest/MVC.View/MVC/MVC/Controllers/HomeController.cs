using MVC.Server;

namespace MVC.Controllers
{
    public class HomeController
    {
        private readonly Router _router;

        public HomeController(Router router)
        {
            _router = router;
        }

        // Trả HTML cho trang Home
        public string Index()
        {
            return File.ReadAllText("Views/HomeView.html");
        }
    }
}
