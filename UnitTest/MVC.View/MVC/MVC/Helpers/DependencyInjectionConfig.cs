namespace MVC.Helpers
{
    // Defines lifetime of a service
    public enum Lifetime
    {
        Singleton,
        Scoped,
        Transient
    }

    public class DependencyInjectionConfig
    {
        internal class ServiceDescriptor
        {
            public Func<DIScope, object> Factory { get; set; }
            public Lifetime Lifetime { get; set; }
            public object Instance { get; set; } // Chỉ dùng cho Singleton
        }

        private readonly Dictionary<Type, ServiceDescriptor> _services = new();

        public void RegisterService<TService, TImplementation>(Lifetime lifetime = Lifetime.Transient)
        {
            _services[typeof(TService)] = new ServiceDescriptor
            {
                Lifetime = lifetime,
                Factory = scope => CreateInstance(typeof(TImplementation), scope)
            };
        }

        public void RegisterFactory<TService>(Lifetime lifetime, Func<DIScope, TService> factory)
        {
            _services[typeof(TService)] = new ServiceDescriptor
            {
                Lifetime = lifetime,
                Factory = scope => factory(scope)
            };
        }

        public void RegisterByType(Type serviceType, Type implementationType, Lifetime lifetime = Lifetime.Transient)
        {
            _services[serviceType] = new ServiceDescriptor
            {
                Lifetime = lifetime,
                Factory = scope => CreateInstance(implementationType, scope)
            };
        }

        internal ServiceDescriptor GetDescriptor(Type type)
        {
            if (!_services.TryGetValue(type, out var descriptor))
                throw new Exception($"Service {type.Name} not registered");
            return descriptor;
        }

        private object CreateInstance(Type type, DIScope scope)
        {
            var ctor = type.GetConstructors().First();
            var parameters = ctor.GetParameters()
                .Select(p => scope.Resolve(p.ParameterType))
                .ToArray();

            return Activator.CreateInstance(type, parameters);
        }
    }

    public class DIScope : IDisposable
    {
        private readonly DependencyInjectionConfig _di;
        private readonly Dictionary<Type, object> _scopedInstances = new();

        public DIScope(DependencyInjectionConfig di)
        {
            _di = di;
        }

        public T Resolve<T>() => (T)Resolve(typeof(T));

        public object Resolve(Type type)
        {
            var descriptor = _di.GetDescriptor(type);

            return descriptor.Lifetime switch
            {
                Lifetime.Singleton => descriptor.Instance ??= descriptor.Factory(this),

                Lifetime.Scoped => _scopedInstances.TryGetValue(type, out var existing)
                    ? existing
                    : _scopedInstances[type] = descriptor.Factory(this),

                Lifetime.Transient => descriptor.Factory(this),

                _ => throw new NotSupportedException()
            };
        }

        public void Dispose()
        {
            _scopedInstances.Clear();
        }
    }
}
