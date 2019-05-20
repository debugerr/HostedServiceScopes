using Microsoft.Extensions.DependencyInjection;
using System;

namespace HostedServiceScopes
{
    public class ScopedProcessorWrapper<T>
        where T : IProcessor
    {
        private readonly IServiceProvider serviceProvider;

        public ScopedProcessorWrapper(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Execute()
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<T>();
                service.DoWork();
            }
        }
    }
}
