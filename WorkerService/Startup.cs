using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WorkerService.Consumers;
using WorkerService.Messaging;
using WorkerService.Models;
using WorkerService.RoutePattern.Builder;
using WorkerService.RoutePattern.InitialRoute;
using WorkerService.RoutePattern.InitialRoute.First;
using WorkerService.RoutePattern.InitialRoute.First.Second;
using WorkerService.RoutePattern.InitialRoute.First.Second.Third;
using IConsumer = WorkerService.Messaging.IConsumer;

namespace WorkerService
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WorkerServiceSettings>(configuration.GetSection(nameof(WorkerServiceSettings)));

            services.TryAddTransient<IProducer, Producer>();
            services.TryAddTransient<IConsumer, Consumer>();

            services.AddTransient<IRouteBuilder, KafkaRouteBuilder>();

            services.TryAddTransient<IKnownMessageConsumer, KnownMessageConsumer>();
            services.TryAddTransient<IUnknownMessageConsumer, UnknownMessageConsumer>();

            SetRoutes(services);
        }

        private void SetRoutes(IServiceCollection services)
        {
            services.AddTransient<IInitialRoute, InitialRoute>();
            services.AddTransient<IInitialRouteUnknown, InitialRouteUnknown>();

            services.AddTransient<IFirstRoute, FirstRoute>();
            services.AddTransient<IFirstRouteUnknown, FirstRouteUnknown>();

            services.AddTransient<ISecondRoute, SecondRoute>();
            services.AddTransient<ISecondRouteUnknown, SecondRouteUnknown>();

            services.AddTransient<IThirdRoute, ThirdRoute>();
        }
    }
}
