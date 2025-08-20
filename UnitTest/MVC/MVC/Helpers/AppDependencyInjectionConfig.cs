using System.Data;
using System.Reflection;
using MVC.Data;
using MVC.Repositories.AccountRepository;
using MVC.Services.AccountService;

namespace MVC.Helpers
{
    public class AppDependencyInjectionConfig
    {
        public static DependencyInjectionConfig CreateConfig()
        {
            var di = new DependencyInjectionConfig();

            // Đăng ký service core
            di.RegisterFactory<IDbConnection>(Lifetime.Scoped, scope => TestDatabaseHelper.GetConnection());
            di.RegisterService<IAccountRepository, AccountRepository>(Lifetime.Scoped);
            di.RegisterService<IAccountService, AccountService>(Lifetime.Transient);

            // Scan toàn bộ controller trong assembly
            RegisterControllers(di);

            return di;
        }
        public static void RegisterControllers(DependencyInjectionConfig di)
        {
            // Scan tất cả assembly đã load trong AppDomain
            var controllerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return new Type[0]; } // tránh lỗi assembly không load được type
                })
                .Where(t => t.Name.EndsWith("Controller") && !t.IsAbstract);

            foreach (var type in controllerTypes)
            {
                // Đăng ký controller vào DI
                di.RegisterByType(type, type, Lifetime.Transient);
            }
        }
    }
}
