using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using TestInfrastructure;
using Xunit;
using Xunit.Abstractions;

namespace Api.CLI.Tests.Demo
{
    public class ApiTraditionalCliTests
    {
        private ServiceCollection serviceCollection;
        private AppSettings appSettings;
        private readonly XUnitLogger _logger;
        private IConfigurationRoot _configuration;
        private HttpClient _httpClient;
        private string message;
        private HttpResponseMessage _httpResponse;
        private string _capturedResponse;
        private LogLevel _capturedLogLevel;
        private EventId _capturedEventId;
        private object _capturedState;
        private Exception _capturedException;

        private WeatherForecast _weatherForecast;


        public ApiTraditionalCliTests(ITestOutputHelper output)
        {
            _logger = new XUnitLogger(output);
        }
        [Fact]
        public async Task ShouldReturnHealthy()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();

            message = "test";
            serviceCollection = new ServiceCollection();
            appSettings = new AppSettings
            {
                AnotherChildAppSetting = new AnotherChildAppSetting { AnotherChildProperty = "-AnotherChildProperty-" },
                ChildAppSetting = new ChildAppSetting { ThisIsAProperty = "-ThisIsAProperty-" }
            };
            var configuration = TestConfigurationProvider.ConfigFromObject(appSettings);
            var webHostBuilder = new WebHostBuilder()
                .UseConfiguration(configuration)
                .ConfigureServices(services => services.AddTransient(x => loggerMock.Object))
                .UseStartup<Startup>();
            var testServer = new TestServer(webHostBuilder);
            var httpClient = testServer.CreateClient();

            var stringContent = new StringContent(message, Encoding.Default, "application/json");
            stringContent.Headers.Add("TestCaseId", new List<string> { $"{Guid.NewGuid()}" });

            //Act
            var httpResponse = await httpClient.PostAsync("/Health", stringContent);
            _ = await httpResponse.Content.ReadAsStringAsync();

            //Assert
            httpResponse.IsSuccessStatusCode.Should().Be(true);
            _logger.WriteLine("Success");
        }

        [Fact]
        public async Task ShouldUpdateWithStatusOK()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();

            var weatherForecast = new WeatherForecast
            {
                TemperatureC = 60
            };

            message = JsonConvert.SerializeObject(weatherForecast);
            serviceCollection = new ServiceCollection();
            appSettings = new AppSettings
            {
                AnotherChildAppSetting = new AnotherChildAppSetting { AnotherChildProperty = "-AnotherChildProperty-" },
                ChildAppSetting = new ChildAppSetting { ThisIsAProperty = "-ThisIsAProperty-" }
            };
            var configuration = TestConfigurationProvider.ConfigFromObject(appSettings);
            var webHostBuilder = new WebHostBuilder()
                .UseConfiguration(configuration)
                .ConfigureServices(services => services.AddTransient(x => loggerMock.Object))
                .UseStartup<Startup>();
            var testServer = new TestServer(webHostBuilder);
            var httpClient = testServer.CreateClient();

            string capturedResponse = string.Empty;
            loggerMock.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback<LogLevel, EventId, object, Exception, object>((logLevel, eventId, state, exception, formatter) =>
                {
                    var invokedMethod = formatter.GetType().GetMethod("Invoke");
                    capturedResponse = (string)invokedMethod.Invoke(formatter, new[] { state, exception });
                    var capturedLogLevel = logLevel;
                    var capturedEventId = eventId;
                    var capturedState = state;
                    var capturedException = exception;
                });

            var stringContent = new StringContent(message, Encoding.Default, "application/json");
            stringContent.Headers.Add("TestCaseId", new List<string> { $"{Guid.NewGuid()}" });

            //Act
            var httpResponse = await httpClient.PostAsync("/WeatherForecast", stringContent);
            _ = await httpResponse.Content.ReadAsStringAsync();

            //Assert
            httpResponse.IsSuccessStatusCode.Should().Be(true);
            weatherForecast.TemperatureC.Should().Be(int.Parse(capturedResponse));

            Func<object, Type, bool> state = (v, t) => v.ToString().Contains(weatherForecast.TemperatureC.ToString());
            loggerMock.Verify(x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once());

            Exception expectedException = null;
            loggerMock.Verify(x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.Is<Exception>(ex => expectedException == null
                                           || ex.GetType() == expectedException.GetType()
                                           && ex.Message == expectedException.Message
                                           && ex.InnerException == expectedException.InnerException),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true))
                , Times.Once());

            _logger.WriteLine("Success");
        }
    }
}
