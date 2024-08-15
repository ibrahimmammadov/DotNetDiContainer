using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDiContainer.ServiceProviders
{
    public class ScopedServiceProvider : ServiceProviderBase, IDisposable
    {
        private readonly ServiceProvider _rootProvider;
        private readonly Dictionary<Type, object> _scopedInstances = new();
        private readonly Dictionary<Type, Func<ScopedServiceProvider, object>> _scopedTypes;

        public ScopedServiceProvider(ServiceProvider rootProvider, Dictionary<Type, Func<ScopedServiceProvider, object>> scopedTypes)
        {
            _rootProvider = rootProvider;
            _scopedTypes = scopedTypes;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public object GetService(Type serviceType)
        {
            if (_scopedInstances.TryGetValue(serviceType, out var service))
            {
                return service;
            }

            if (_scopedTypes.TryGetValue(serviceType, out var factory))
            {
                service = factory.Invoke(this);
                _scopedInstances[serviceType] = service;
                return service;
            }

            return _rootProvider.GetService(serviceType)!;
        }

        public void Dispose()
        {
            _scopedInstances.Clear();
        }
    }
}
