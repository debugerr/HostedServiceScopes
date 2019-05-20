using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HostedServiceScopes
{
    public class BackgroundServiceWrapper<T>
        where T : IProcessor
    {
        private IHostBuilder hostBuilder;
        public IHostBuilder Setup(string[] args)
        {
            this.hostBuilder = new HostBuilder()
                .ConfigureLogging((ctx, config) =>
                {
                    config.AddConsole();
                    config.AddDebug();
                })
                .ConfigureHostConfiguration(cfg =>
                {
                    cfg.AddEnvironmentVariables();
                })
                .ConfigureAppConfiguration((ctx, config) =>
                {
                    config.AddJsonFile("appsettings.json", true);
                    config.AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging();

                    services.AddSingleton<MainProcessorLoop>();
                    services.AddScoped(typeof(T));
                    services.AddScoped((sp) => (IProcessor)sp.GetService(typeof(T)));
                    services.AddScoped<ScopedProcessorWrapper<IProcessor>>();
                })
                .UseConsoleLifetime();

            return this.hostBuilder;
        }

        public async Task StartAsync()
        {
            using (var host = this.hostBuilder.Build())
            {
                await host.StartAsync();
                var loop = host.Services.GetRequiredService<MainProcessorLoop>();
                loop.Start();
                await host.WaitForShutdownAsync();
            }
        }
    }
}
