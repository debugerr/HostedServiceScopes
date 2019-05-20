using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HostedServiceScopes
{
    public interface IService
    {
        Task<object> GetInformationAsync();
    }

    public class MyService : IService
    {
        public Task<object> GetInformationAsync()
        {
            return Task.FromResult<object>("service information");
        }
    }

    public class AProcessor : IProcessor, IDisposable
    {
        private readonly ILogger<AProcessor> logger;
        private readonly IService myService;

        public AProcessor(ILogger<AProcessor> logger, IService myService)
        {
            this.logger = logger;
            this.myService = myService;
        }

        public async Task DoWork()
        {
            this.logger.LogInformation($"{nameof(AProcessor)}:{nameof(DoWork)}");
            var info = await this.myService.GetInformationAsync();
            this.logger.LogInformation((string)info);
        }

        public void Dispose()
        {
            this.logger.LogInformation($"{nameof(AProcessor)}:{nameof(Dispose)}");
        }
    }
}
