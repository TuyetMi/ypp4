
namespace MVC.Helpers
{
    // Defines lifetime of a service
    public enum Lifetime
    {
        Singleton,  // Only one instance for the whole application
        Scoped,     // One instance per scope (e.g., per test or per request)
        Transient   // New instance every time it's resolved
    }

    public class DependencyInjectionConfig
    {
        // Describes how a service is registered
        internal class ServiceDescriptor
        {
            public Func<DIScope, object> Factory { get; set; } // Factory method to create the instance
            public Lifetime Lifetime { get; set; }            // Lifetime of this service
            public object Instance { get; set; }              // Stores instance for Singleton
        }

        private readonly Dictionary<Type, ServiceDescriptor> _services = new();

        // 1. Interface → Implementation
        public void RegisterService<TService, TImplementation>(Lifetime lifetime = Lifetime.Transient)
        {
            _services[typeof(TService)] = new ServiceDescriptor
            {
                Lifetime = lifetime,
                Factory = scope => CreateInstance(typeof(TImplementation), scope)
            };
        }

        // 2. Register với Factory
        public void RegisterFactory<TService>(Lifetime lifetime, Func<DIScope, TService> factory)
        {
            _services[typeof(TService)] = new ServiceDescriptor
            {
                Lifetime = lifetime,
                Factory = scope => factory(scope)
            };
        }

        // 3. Non-generic Register (dùng cho scan/reflection)
        public void RegisterByType(Type serviceType, Type implementationType, Lifetime lifetime = Lifetime.Transient)
        {
            _services[serviceType] = new ServiceDescriptor
            {
                Lifetime = lifetime,
                Factory = scope => CreateInstance(implementationType, scope)
            };
        }

        // Generic resolve method
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        // Resolve by runtime Type
        public object Resolve(Type type)
        {
            if (!_services.TryGetValue(type, out var descriptor))
                throw new Exception($"Service {type.Name} not registered");

            if (descriptor.Lifetime == Lifetime.Singleton)
                return descriptor.Instance ??= descriptor.Factory(null);

            // Transient
            return descriptor.Factory(null);
        }

        // Internal: get descriptor by type
        internal ServiceDescriptor GetDescriptor(Type type)
        {
            if (!_services.ContainsKey(type))
                throw new Exception($"Service {type.Name} not registered");
            return _services[type];
        }

        // Create instance by resolving constructor dependencies
        private object CreateInstance(Type type, DIScope scope)
        {
            var ctor = type.GetConstructors().First();
            var parameters = ctor.GetParameters()
                .Select(p =>
                {
                    if (scope != null)
                        return scope.Resolve(p.ParameterType); // Resolve scoped dependencies
                    return Resolve(p.ParameterType);          // Resolve singleton/transient
                })
                .ToArray();

            return Activator.CreateInstance(type, parameters); // Create object
        }
    }

    // Scope container for managing Scoped services
    public class DIScope : IDisposable
    {
        private readonly DependencyInjectionConfig _di;
        private readonly Dictionary<Type, object> _scopedInstances = new();

        public DIScope(DependencyInjectionConfig di)
        {
            _di = di;
        }

        // Generic resolve
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        // Resolve by runtime Type
        public object Resolve(Type type)
        {
            var descriptor = _di.GetDescriptor(type);

            if (descriptor.Lifetime == Lifetime.Scoped)
            {
                if (!_scopedInstances.ContainsKey(type))
                    _scopedInstances[type] = descriptor.Factory(this); // Create scoped instance once per scope
                return _scopedInstances[type];
            }

            return _di.Resolve(type); // Delegate to main DI for Singleton/Transient
        }

        public void Dispose()
        {
            _scopedInstances.Clear(); // Clear scoped instances when done
        }
    }
}
