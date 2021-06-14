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

namespace Api.CLI.Tests
{
    public class ApiTestsCli
    {
        private Mock<ILogger<WeatherForecastController>> _loggerMock = new Mock<ILogger<WeatherForecastController>>();
        private ServiceCollection _serviceCollection;
        private AppSettings _appSettings;
        private readonly XUnitLogger _logger;
        private IConfigurationRoot _configuration;
        private HttpClient _httpClient;
        private string _message;
        private HttpResponseMessage _httpResponse;
        private string _capturedResponse;
        private LogLevel _capturedLogLevel;
        private EventId _capturedEventId;
        private object _capturedState;
        private Exception _capturedException;

        private WeatherForecast _weatherForecast;


        public ApiTestsCli(ITestOutputHelper output)
        {
            _logger = new XUnitLogger(output);
        }
        [Fact]
        public async Task ShouldReturnHealthy()
        {
            GivenAMessage("test");
            GivenAServiceCollection();
            GivenAnAppSettings();
            GivenAConfiguration();
            GivenATestServerClient(GivenValidMockServices);

            await WhenAValidResponseIsReturnedFromTheClient("/Health");

            ThenTheHttpResponseShouldBe(true);
            _logger.WriteLine("Success");
        }

        [Fact]
        public async Task ShouldUpdateWithStatusOK()
        {
            _weatherForecast = new WeatherForecast
            {
                TemperatureC = 60
            };

            GivenAMessage(JsonConvert.SerializeObject(_weatherForecast));
            GivenAServiceCollection();
            GivenAnAppSettings();
            GivenAConfiguration();
            GivenATestServerClient(GivenValidMockServices);
            GivenALogMessageIsCaptured();

            await WhenAValidResponseIsReturnedFromTheClient("/WeatherForecast");

            ThenTheHttpResponseShouldBe(true);
            ThenTheTemperatureIsTheSame();
            ThenTheLoggerWasCalled(_weatherForecast.TemperatureC.ToString(), LogLevel.Information, Times.Once());
            ThenTheLoggerWasCalledWithException(LogLevel.Information, null, s => s == _weatherForecast.TemperatureC.ToString(), Times.Once());
            _logger.WriteLine("Success");
        }

        private void ThenTheLoggerWasCalled(string expectedMessage, LogLevel expectedLogLevel, Times times)
        {
            Func<object, Type, bool> state = (v, t) => v.ToString().Contains(expectedMessage);

            _loggerMock.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == expectedLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => state(v, t)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                times);
        }

        private void ThenTheLoggerWasCalledWithException(LogLevel logLevel, Exception expectedException, Predicate<string> predicate, Times expectedTimes)
        {
            _loggerMock.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == logLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => predicate(v.ToString())),
                It.Is<Exception>(ex => expectedException == null
                                       || ex.GetType() == expectedException.GetType()
                                       && ex.Message == expectedException.Message
                                       && ex.InnerException == expectedException.InnerException),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true))
                , expectedTimes);
        }

        private void ThenTheTemperatureIsTheSame()
        {
            _weatherForecast.TemperatureC.Should().Be(int.Parse(_capturedResponse));
        }

        private void GivenALogMessageIsCaptured()
        {
            _loggerMock.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback<LogLevel, EventId, object, Exception, object>((logLevel, eventId, state, exception, formatter) =>
                {
                    var invokedMethod = formatter.GetType().GetMethod("Invoke");
                    _capturedResponse = (string)invokedMethod.Invoke(formatter, new[] { state, exception });
                    _capturedLogLevel = logLevel;
                    _capturedEventId = eventId;
                    _capturedState = state;
                    _capturedException = exception;
                });
        }

        private void ThenTheHttpResponseShouldBe(bool isSuccess)
        {
            _httpResponse.IsSuccessStatusCode.Should().Be(isSuccess);
        }

        private void GivenAMessage(string test)
        {
            _message = test;
        }

        private async Task WhenAValidResponseIsReturnedFromTheClient(string url)
        {
            var stringContent = new StringContent(_message, Encoding.Default, "application/json");
            stringContent.Headers.Add("TestCaseId", new List<string> { $"{Guid.NewGuid()}" });

            _httpResponse = await _httpClient.PostAsync(url, stringContent);
            _ = await _httpResponse.Content.ReadAsStringAsync();
        }

        private void GivenValidMockServices(IServiceCollection services)
        {
            services.AddTransient(x => _loggerMock.Object);
        }

        private void GivenATestServerClient(Action<IServiceCollection> mockObjects)
        {
            var webHostBuilder = new WebHostBuilder()
                .UseConfiguration(_configuration)
                .ConfigureServices(mockObjects)
                .UseStartup<Startup>();
            var testServer = new TestServer(webHostBuilder);
            _httpClient = testServer.CreateClient();
        }

        private void GivenAServiceCollection()
        {
            _serviceCollection = new ServiceCollection();
        }

        private void GivenAnAppSettings()
        {
            _appSettings = new AppSettings
            {
                AnotherChildAppSetting = new AnotherChildAppSetting { AnotherChildProperty = "-AnotherChildProperty-" },
                ChildAppSetting = new ChildAppSetting { ThisIsAProperty = "-ThisIsAProperty-" }
            };
        }

        private void GivenAConfiguration()
        {
            _configuration = TestConfigurationProvider.ConfigFromObject(_appSettings);
        }
    }
}
