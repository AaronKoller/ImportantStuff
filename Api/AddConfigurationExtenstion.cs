using System.Linq;
using Api.Models;
using Api.Models.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api
{
    public static class AddConfigurationExtenstion
    {

        public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            //early exit if generic logger is found
            //if (services.Any(x => x.ServiceType.Name == typeof(ILogger<>).Name)) return;

            //early exit if logger is found
            if (services.Any(x => x.ServiceType == typeof(ILogger))) return;

            //configuration example1 - get modify and add IOptions
            var childAppSettings = configuration.GetSection(nameof(ChildAppSetting)).Get<ChildAppSetting>();

            if (childAppSettings.ThisIsAProperty.Equals("throw"))
                throw new ExampleException("get out of here");

            childAppSettings.ThisIsAProperty = "Modified" + childAppSettings.ThisIsAProperty;
            services.AddTransient(x => Options.Create(childAppSettings));

            //configuration example2 - standard get section
            services.Configure<AnotherChildAppSetting>(configuration.GetSection(nameof(AnotherChildAppSetting)));

            //configuration example3 - options created manually
            services.Configure<AggregateAppSetting>(options =>
            {
                var provider = services.BuildServiceProvider();
                var test = provider.GetRequiredService<IOptions<AnotherChildAppSetting>>();
                options.TogetherWeAreStronger = childAppSettings.ThisIsAProperty + test.Value.AnotherChildProperty;
            });
        }
    }
}
