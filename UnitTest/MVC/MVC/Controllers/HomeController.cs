using MVC.Server;

namespace MVC.Controllers
{
    public class HomeController
    {
        [HttpGet]
        public string Index()
        {
            return "Welcome to my custom MVC server ";
        }
        [HttpGet]
        public object Info()
        {
            return new
            {
                Name = "My Custom MVC Framework",
                Version = "1.0",
                Author = "Mi Luong 😊",
                Timestamp = DateTime.Now
            };
        }
        // Helper gọi view file
        private string View(string viewPath)
        {
            var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views", viewPath + ".html");
            if (File.Exists(fullPath))
            {
                return File.ReadAllText(fullPath);
            }
            return $"<h1>View {viewPath} not found</h1>";
        }
    }
}
