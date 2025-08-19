

using MVC.Data;
using MVC.Repositories.AccountRepository;
using MVC.Services.AccountService;
using System.Data;

namespace MVC.Helpers
{
    [TestClass]
    public abstract class TestStartUp
    {
        protected DependencyInjectionConfig _di;
        protected DIScope _scope;

        [TestInitialize]
        public virtual void StartUp()
        {
            TestDatabaseHelper.InitDatabase();

            _di = new DependencyInjectionConfig();
            _di.Register<IDbConnection>(Lifetime.Scoped, scope => TestDatabaseHelper.GetConnection());
            _di.Register<IAccountRepository, AccountRepository>(Lifetime.Scoped);
            _di.Register<IAccountService, AccountService>(Lifetime.Transient);

            _scope = new DIScope(_di);
        }

        [TestCleanup]
        public virtual void BaseCleanup()
        {
            _scope.Dispose();
            TestDatabaseHelper.CloseDatabase();
        }
    }
}
