using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkerService.Consumers;
using WorkerService.Models;
using WorkerService.RoutePattern.Builder;
using WorkerService.RoutePattern.InitialRoute;
using IConsumer = WorkerService.Messaging.IConsumer;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConsumer _consumer;
        private readonly IRouteBuilder _routeBuilder;
        private readonly IInitialRoute _initialRoute;
        private readonly IUnknownMessageConsumer _unknownMessageConsumer;
        private readonly WorkerServiceSettings _workerServiceSettings;


        public Worker(ILogger<Worker> logger, IOptions<WorkerServiceSettings> workerServiceSettings, IConsumer consumer, IRouteBuilder routeBuilder, IInitialRoute initialRoute)
        {
            _logger = logger;
            _consumer = consumer;
            _routeBuilder = routeBuilder;
            _initialRoute = initialRoute;
            _workerServiceSettings = workerServiceSettings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                Parallel.For(0, _workerServiceSettings.NumOfConsumers, async (i, state) =>
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        Message message = null;
                        try
                        {
                            message = _consumer.Get();
                            message.Route = _routeBuilder.Build(message);
                            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                            await _initialRoute.MoveNext(message);
                        }
                        catch (Exception e)
                        {
                            await _unknownMessageConsumer.Consume(message);
                        }
                    }
                });
            }, stoppingToken);
        }
    }
}
