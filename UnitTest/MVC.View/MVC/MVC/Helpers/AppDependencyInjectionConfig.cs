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

            // Core services
            di.RegisterFactory<IDbConnection>(Lifetime.Scoped, _ => TestDatabaseHelper.GetConnection());
            di.RegisterService<IAccountRepository, AccountRepository>(Lifetime.Scoped);
            di.RegisterService<IAccountService, AccountService>(Lifetime.Transient);

            // Scan & register controllers
            RegisterControllers(di);

            return di;
        }

        private static void RegisterControllers(DependencyInjectionConfig di)
        {
            var controllerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Name.EndsWith("Controller") && !t.IsAbstract);

            foreach (var type in controllerTypes)
            {
                di.RegisterByType(type, type, Lifetime.Transient);
            }
        }
    }
}
