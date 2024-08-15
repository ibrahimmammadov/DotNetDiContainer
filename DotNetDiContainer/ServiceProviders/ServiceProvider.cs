using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDiContainer.ServiceProviders
{
    public class ServiceProvider : ServiceProviderBase// : IServiceProvider
    {
        private readonly Dictionary<Type, Func<object>> _transientTypes = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, Lazy<object>> _singletonTypes = new Dictionary<Type, Lazy<object>>();
        private readonly Dictionary<Type, Func<ScopedServiceProvider, object>> _scopedTypes = new();
        private readonly ServiceProvider _rootProvider;

        public ServiceProvider(ServiceCollection servicesCollection)
        {
            _rootProvider = this;
            GenerateServices(servicesCollection);
        }

        private void GenerateServices(ServiceCollection servicesCollection)
        {
            foreach (var serviceDescriptor in servicesCollection)
            {
                switch (serviceDescriptor.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        if (serviceDescriptor.Implementation is not null)
                        {
                            _singletonTypes[serviceDescriptor.ServiceType] = new Lazy<object>(serviceDescriptor.Implementation);
                            continue;
                        }

                        if (serviceDescriptor.ImplementationFactory is not null)
                        {
                            _singletonTypes[serviceDescriptor.ServiceType] = new Lazy<object>(() =>
                            serviceDescriptor.ImplementationFactory(this));
                            continue;
                        }
                        _singletonTypes[serviceDescriptor.ServiceType] = new Lazy<object>(() => Activator.CreateInstance
                            (serviceDescriptor.ImplementationType, GetConstructorParameters(serviceDescriptor.ImplementationType, this))!);
                        continue;

                    case ServiceLifetime.Transient:
                        if (serviceDescriptor.ImplementationFactory is not null)
                        {
                            _transientTypes[serviceDescriptor.ServiceType] = () =>
                            serviceDescriptor.ImplementationFactory(this);
                            continue;
                        }
                        _transientTypes[serviceDescriptor.ServiceType] = () => Activator.CreateInstance
                            (serviceDescriptor.ImplementationType, GetConstructorParameters(serviceDescriptor.ImplementationType, this))!;
                        continue;
                    case ServiceLifetime.Scoped:
                        if (serviceDescriptor.ImplementationFactory != null)
                        {
                            _scopedTypes[serviceDescriptor.ServiceType] = (sp) => serviceDescriptor.ImplementationFactory(_rootProvider);
                            continue;
                        }

                        _scopedTypes[serviceDescriptor.ServiceType] = (sp) =>
                        Activator.CreateInstance(serviceDescriptor.ImplementationType, sp.GetConstructorParameters(serviceDescriptor.ImplementationType, _rootProvider))!;
                        continue;
                }
            }
        }


        public T? GetService<T>()
        {
            return (T?)GetService(typeof(T));
        }
        public object? GetService(Type serviceType)
        {
            var service = _singletonTypes.GetValueOrDefault(serviceType);
            if (service is not null)
            {
                return service.Value;
            }
            var transientService = _transientTypes.GetValueOrDefault(serviceType);
            return transientService?.Invoke();

        }

        public ScopedServiceProvider CreateScope()
        {
            return new ScopedServiceProvider(this, _scopedTypes);
        }
    }
}
