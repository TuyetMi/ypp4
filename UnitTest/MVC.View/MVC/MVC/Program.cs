using MVC.Helpers;
using MVC.Data;
using MVC.Server;
using MVC.Models; // Giả sử DIScope, DI config, DBHelper ở đây

class Program
{
    static async Task Main(string[] args)
    {
        // 1. Prepare database
        TestDatabaseHelper.InitDatabase();

        // 2. Configure DI
        var diConfig = AppDependencyInjectionConfig.CreateConfig();
        using var scope = new DIScope(diConfig);

        // Router
        var router = new Router(diConfig);

        // HttpServer lắng nghe localhost:5000
        var server = new HttpServer(
            new string[] { "http://localhost:5000/" },
            router
        );

        // Chạy server
        server.Start();
    }
}
