using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace TestInfrastructure
{
    public class AppSettingsProvider
    {
        public IConfigurationRoot ConfigurationRoot;

        public IConfigurationRoot GetAppSettings()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            ConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile($"AppSettings/appsettings{TestEnvironment.Local}.json", optional: false)
                .Build();

            var environment = ConfigurationRoot["Environment"];
            Console.WriteLine($"Testing Environment Scope: {environment}");
            if (environment.StartsWith("#{"))
            {
                ConfigurationRoot = new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile($"AppSettings/appsettings{TestEnvironment.Pipeline}.json", optional: false)
                    .Build();
            }

            return ConfigurationRoot;
        }
    }

    public enum TestEnvironment
    {
        Local,
        Pipeline
    }
}
