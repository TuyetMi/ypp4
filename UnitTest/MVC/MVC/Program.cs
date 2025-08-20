using MVC.Helpers;
using MVC.Data;
using MVC.Server; // Giả sử DIScope, DI config, DBHelper ở đây

class Program
{
    static async Task Main()
    {
        TestDatabaseHelper.InitDatabase();

        // ===== Khởi tạo DI =====
        var diConfig = AppDependencyInjectionConfig.CreateConfig();
        using var scope = new DIScope(diConfig);

        // ===== Tạo Router tự động ánh xạ controller/action =====
        var router = new Router(scope);

        // ===== Khởi tạo HTTP server =====
        var server = new HttpServer(router);
        await server.StartAsync("http://localhost:5000/");
    }
}
