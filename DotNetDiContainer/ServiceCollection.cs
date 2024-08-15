using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetDiContainer.ServiceProviders;

namespace DotNetDiContainer
{
    public class ServiceCollection:List<ServiceDescriptor>
    {
        public ServiceCollection AddService(ServiceDescriptor serviceDescriptor)
        {
            Add(serviceDescriptor);
            return this;
        }
        public ServiceCollection AddScoped<TService>() where TService : class
        {
            ServiceDescriptor serviceDescriptor = ServiceDescriptorWithLifetime<TService, TService>(ServiceLifetime.Scoped);
            Add(serviceDescriptor);
            return this;
        }

        public ServiceCollection AddScoped<TService, TImplementation>() where TService : class where TImplementation : class, TService
        {
            ServiceDescriptor serviceDescriptor = ServiceDescriptorWithLifetime<TService, TImplementation>(ServiceLifetime.Scoped);
            Add(serviceDescriptor);
            return this;
        }

        public ServiceCollection AddSingleton<IService>(Func<ServiceProvider, IService> factory) where IService : class
        {
            var serviceDescriptor = new ServiceDescriptor
            {
                ServiceType = typeof(IService),
                ImplementationType = typeof(IService),
                ImplementationFactory = factory,
                Lifetime = ServiceLifetime.Singleton
            };
            Add(serviceDescriptor);
            return this;
        }

        public ServiceCollection AddTransient<IService>(Func<ServiceProvider, IService> factory) where IService : class
        {
            var serviceDescriptor = new ServiceDescriptor
            {
                ServiceType = typeof(IService),
                ImplementationType = typeof(IService),
                ImplementationFactory = factory,
                Lifetime = ServiceLifetime.Transient
            };
            Add(serviceDescriptor);
            return this;
        }

        public ServiceCollection AddSingleton(object implementation)
        {
            var serviceType = implementation.GetType();
            var serviceDescriptor =  new ServiceDescriptor
            {
                ServiceType = serviceType,
                ImplementationType = serviceType,
                Implementation = implementation,
                Lifetime = ServiceLifetime.Singleton
            };
            Add(serviceDescriptor);
            return this;
        }

        public ServiceCollection AddSingleton<TService>() where TService : class
        {
            ServiceDescriptor serviceDescriptor = ServiceDescriptorWithLifetime<TService, TService>(ServiceLifetime.Singleton);
            Add(serviceDescriptor);
            return this;
        }

        public ServiceCollection AddSingleton<TService,TImplementation>() where TService : class where TImplementation : class,TService
        {
            ServiceDescriptor serviceDescriptor = ServiceDescriptorWithLifetime<TService, TImplementation>(ServiceLifetime.Singleton);
            Add(serviceDescriptor);
            return this;
        }

        public ServiceCollection AddTransient<TService>() where TService : class
        {
            ServiceDescriptor serviceDescriptor = ServiceDescriptorWithLifetime<TService, TService>(ServiceLifetime.Transient);
            Add(serviceDescriptor);
            return this;
        }

        public ServiceCollection AddTransient<TService,TImplementation>() where TService:class where TImplementation : class,TService
        {
            ServiceDescriptor serviceDescriptor = ServiceDescriptorWithLifetime<TService, TImplementation>(ServiceLifetime.Transient);
            Add(serviceDescriptor);
            return this;
        }

        public ServiceProvider BuildServiceProvider()
        {
            return new ServiceProvider(this);
        }

        private static ServiceDescriptor ServiceDescriptorWithLifetime<TService, TImplementation>(ServiceLifetime lifetime)
        {
            return new ServiceDescriptor
            {
                ServiceType = typeof(TService),
                ImplementationType = typeof(TImplementation),
                Lifetime = lifetime
            };
        }
    }
}
