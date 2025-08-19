
using MVC.Data;
using MVC.Repositories.AccountRepository;
using MVC.Services.AccountService;


namespace MVC.Helpers
{
    // chưa quản lý time - vong đời

    public enum Lifetime
    {
        Singleton, //only one instance is created and reused everywhere
        Transient,  //a new instance is created every time it’s resolved
        Scoped
    }

    // Describes how a service is registered:
    public class DependencyInjectionConfig
    {
        // Thông tin 1 service đã đăng ký
        private class ServiceDescriptor
        {
            public Func<object> Factory { get; set; } = null!;      //Factory → a function that knows how to create the object.
            public Lifetime Lifetime { get; set; }                  //Lifetime → whether it’s Singleton or Transient.
            public object? Instance { get; set; }                   //Instance → holds the created instance (only for Singleton).
        }

        private static readonly Dictionary<Type, ServiceDescriptor> _services = new();

        // When the class is used for the first time, it registers default services:
        static DependencyInjectionConfig()
        {

            Register<IAccountRepository>(() =>
                new AccountRepository(TestDatabaseHelper.GetConnection()), Lifetime.Transient); // always creates a new AccountRepository.

            Register<IAccountService>(() =>
                new AccountService(Resolve<IAccountRepository>()), Lifetime.Transient); //creates a new AccountService, injecting an IAccountRepository

        }

        // Đăng ký service mới
        public static void Register<T>(Func<object> factory, Lifetime lifetime = Lifetime.Transient) // T = interface or type.
        {
            _services[typeof(T)] = new ServiceDescriptor
            {
                Factory = factory,      // factory = how to build the object.
                Lifetime = lifetime     // lifetime = default is Transient.
            };
        }

        // Resolve generic
        public static T Resolve<T>()
        {
            return (T)Resolve(typeof(T)); // Resolves (gets) a service instance by type.
        }

        // Resolve theo Type
        private static object Resolve(Type type)
        {
            if (!_services.TryGetValue(type, out var descriptor))
                throw new Exception($"No service registered for type {type.Name}");

            return descriptor.Lifetime switch
            {
                Lifetime.Singleton => descriptor.Instance ??= descriptor.Factory(),
                Lifetime.Transient => descriptor.Factory(),
                _ => throw new NotImplementedException($"Lifetime {descriptor.Lifetime} not supported")
            };
        }


    }
}
