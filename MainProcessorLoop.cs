using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HostedServiceScopes
{
    public class MainProcessorLoop
    {
        private readonly ScopedProcessorWrapper<IProcessor> processor;
        private CancellationToken cancellationToken;
        private TimeSpan taskDelay;

        public MainProcessorLoop(IApplicationLifetime appLifetime, ScopedProcessorWrapper<IProcessor> processor, IConfiguration config)
        {
            this.processor = processor;
            this.cancellationToken = appLifetime.ApplicationStopping;
            this.taskDelay = TimeSpan.FromSeconds(config.GetValue<int>("TaskDelay", 1));
        }

        public void Start()
        {
            Task.Run(() => MainLoop());
        }

        public void MainLoop()
        {
            while (!this.cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep((int)this.taskDelay.TotalMilliseconds);
                this.processor.Execute();
                Console.WriteLine(".");
            }
        }
    }
}
