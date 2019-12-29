using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebSiteStatus
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient _client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await _client.GetAsync("https://www.google.com");

                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"The WebSite is UP with status: {result.StatusCode}");
                }
                else
                {
                    _logger.LogError($"The WebSite is DOWN with status: {result.StatusCode}");
                }
                
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
