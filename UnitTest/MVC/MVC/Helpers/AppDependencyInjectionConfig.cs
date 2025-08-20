using System.Data;
using System.Reflection;
using MVC.Data;
using MVC.Repositories.AccountRepository;
using MVC.Services.AccountService;
using static MVC.Helpers.DependencyInjectionConfig;

namespace MVC.Helpers
{
    public class AppDependencyInjectionConfig
    {
        public static DependencyInjectionConfig CreateConfig()
        {
            var di = new DependencyInjectionConfig();

            // Đăng ký các service chung cho mọi test
            di.Register<IDbConnection>(Lifetime.Scoped, scope => TestDatabaseHelper.GetConnection());
            di.Register<IAccountRepository, AccountRepository>(Lifetime.Scoped);
            di.Register<IAccountService, AccountService>(Lifetime.Transient);

            // Scan toàn bộ controller trong assembly
            RegisterControllers(di);

            return di;
        }
        public static void RegisterControllers(DependencyInjectionConfig di)
        {
            // Lấy tất cả type trong assembly hiện tại
            var controllerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Name.EndsWith("Controller") && !t.IsAbstract);

            foreach (var type in controllerTypes)
            {
                // Đăng ký chính nó vào DI
                di.Register(type, type, Lifetime.Transient);
            }
        }
    }
}
