using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDiContainer.ServiceProviders
{
    public class ServiceProviderBase
    {
        public object?[] GetConstructorParameters(Type implementationType, ServiceProvider rootProvider)
        {
            var constructorInfo = implementationType.GetConstructors().First();
            var parameters = constructorInfo.GetParameters()
                .Select(p => rootProvider.GetService(p.ParameterType)).ToArray();
            return parameters;
        }
    }
}
