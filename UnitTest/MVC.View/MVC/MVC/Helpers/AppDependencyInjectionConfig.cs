using System.Data;
using System.Reflection;
using MVC.Repositories.AccountRepository;
using MVC.Repositories.WorkspaceRepository;
using MVC.Services.AccountService;
using MVC.Services.WorkspaceService;

namespace MVC.Helpers
{
    public class AppDependencyInjectionConfig
    {
        public static DependencyInjectionConfig CreateConfig()
        {
            var di = new DependencyInjectionConfig();

            // Core services
            di.RegisterFactory<IDbConnection>(Lifetime.Scoped, _ => TestDatabaseHelper.GetConnection());

            // Account 
            di.RegisterService<IAccountRepository, AccountRepository>(Lifetime.Scoped);
            di.RegisterService<IAccountService, AccountService>(Lifetime.Transient);

            // Workspace
            di.RegisterService<IWorkspaceRepository, WorkspaceRepository>(Lifetime.Scoped);
            di.RegisterService<IWorkspaceService, WorkspaceService>(Lifetime.Transient);

            // Scan & register controllers
            RegisterControllers(di);

            return di;
        }

        private static void RegisterControllers(DependencyInjectionConfig di)
        {
            // Lấy assembly chứa các controller (có thể thay bằng nhiều assembly nếu cần)
            var assembly = Assembly.GetExecutingAssembly();

            // Tìm tất cả class kết thúc bằng "Controller" và không abstract
            var controllerTypes = assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Controller") && !t.IsAbstract);

            foreach (var type in controllerTypes)
            {
                di.RegisterByType(type, type, Lifetime.Transient);
            }
        }

    }
}
