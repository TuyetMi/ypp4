using MVC.Helpers;
using MVC.Data;
using MVC.Server; // Giả sử DIScope, DI config, DBHelper ở đây

class Program
{
    static async Task Main(string[] args)
    {
        // 1. Prepare database
        TestDatabaseHelper.InitDatabase();

        // 2. Configure DI
        var diConfig = AppDependencyInjectionConfig.CreateConfig();
        using var scope = new DIScope(diConfig);

        // 3. Setup router & server
        var router = new Router(scope);
        var server = new HttpServer(router);

        // 4. Start server
        await server.StartAsync("http://localhost:5000/");
    }
}
