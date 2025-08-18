
using MVC.Data;
using MVC.Repositories.AccountRepository;
using MVC.Services.AccountService;


namespace MVC.Helpers
{
    public enum Lifetime
    {
        Singleton,
        Transient
    }

    public class DependencyInjectionConfig
    {
        private class ServiceDescriptor
        {
            public Func<object> Factory { get; set; } = null!;
            public Lifetime Lifetime { get; set; }
            public object? Instance { get; set; } // Lưu instance nếu Singleton
        }

        private static readonly Dictionary<Type, ServiceDescriptor> _services = new();

        static DependencyInjectionConfig()
        {

            Register<IAccountRepository>(() =>
                new AccountRepository(TestDatabaseHelper.GetConnection()), Lifetime.Transient);

            Register<IAccountService>(() =>
                new AccountService(Resolve<IAccountRepository>()), Lifetime.Transient);

        }

        // Đăng ký service mới
        public static void Register<T>(Func<object> factory, Lifetime lifetime = Lifetime.Transient)
        {
            _services[typeof(T)] = new ServiceDescriptor
            {
                Factory = factory,
                Lifetime = lifetime
            };
        }

        // Resolve generic
        public static T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        // Resolve theo Type
        private static object Resolve(Type type)
        {
            if (_services.TryGetValue(type, out var descriptor))
            {
                if (descriptor.Lifetime == Lifetime.Singleton)
                {
                    if (descriptor.Instance == null)
                        descriptor.Instance = descriptor.Factory();
                    return descriptor.Instance;
                }
                else // Transient
                {
                    return descriptor.Factory();
                }
            }

            throw new Exception($"No service registered for type {type.Name}");
        }
    }
}
