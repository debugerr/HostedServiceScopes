using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HostedServiceScopes
{
    class Program
    {
        public static void Main(string[] args)
        {
            var serviceWrapper = new BackgroundServiceWrapper<AProcessor>();
            serviceWrapper.Setup(args)
                .ConfigureServices(services => 
                {
                    // add my dependencies
                    services.AddSingleton<IService, MyService>();
                });

            serviceWrapper.StartAsync().GetAwaiter().GetResult();
        }
    }
}
