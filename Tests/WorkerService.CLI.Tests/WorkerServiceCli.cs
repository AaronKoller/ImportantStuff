using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestInfrastructure;
using WorkerService.Messaging;
using WorkerService.Models;
using Xunit;

namespace WorkerService.CLI.Tests
{
    public class WorkerServiceCli
    {
        private WorkerAppSettings _appSettings;
        private IConfigurationRoot _configuration;
        private IHost _genericHost;

        private Mock<IConsumer> _consumerMock = new Mock<IConsumer>();
        private Mock<IProducer> _producerMock = new Mock<IProducer>();
        private Message _message;
        private string _capturedBody;

        [Fact]
        public async Task ShouldSendAndCaptureMessage()
        {
            GivenAMessage();
            GivenTheConsumerReturnsAMessage();
            GivenASentStringIsCapturedFromTheProducer();

            GivenAnAppSettings();
            GivenAConfiguration();

            GivenAGenericHost(SetupHappyPathMocks);

            await WhenGenericHostIsStarted();

            using (_genericHost)
            {
                await WaitForProcessing();
                ThenTheCapturedBodyIsTheSameAsTheMessageSent();
            }
        }

        private async Task WaitForProcessing()
        {
            await WaitFor(TimeSpan.FromMilliseconds(10), new CancellationTokenSource(1000).Token, () => _capturedBody is null);
        }

        //the cancellation token ensures that if the stopCondition is not met, that we can still exit the while loop with a timeout token
        private async Task WaitFor(TimeSpan timespan, CancellationToken token, Func<bool> stopCondition)
        {
            while (stopCondition.Invoke())
            {
                try
                {
                    await Task.Delay(timespan, token);
                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        private void ThenTheCapturedBodyIsTheSameAsTheMessageSent()
        {
            _message.Body.Should().BeEquivalentTo(_capturedBody);
        }

        private void GivenASentStringIsCapturedFromTheProducer()
        {
            _producerMock.Setup(pm => pm.Send(_message.Body)).Callback<string>((s) =>
                {
                    _capturedBody = s;
                }
            );
        }

        private void GivenTheConsumerReturnsAMessage()
        {
            _consumerMock.Setup(cm => cm.Get()).Returns(_message);
        }

        private void GivenAMessage()
        {
            _message = new Message
            {
                Body = "\"PropertyTest\": \"ValueTest\"",
                Headers = new Dictionary<string, object>
                {
                    {
                        "Topic", "First.Second.Third"
                    }
                }
            };
        }

        private async Task WhenGenericHostIsStarted()
        {
            await _genericHost.StartAsync();
        }

        private void GivenAGenericHost(Action<IServiceCollection> mockServices)
        {
            _genericHost = GivenATestHostBuilder(mockServices).Build();
        }

        private IHostBuilder GivenATestHostBuilder(Action<IServiceCollection> mockServices)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(mockServices);
        }

        private void SetupHappyPathMocks(IServiceCollection services)
        {
            services.AddTransient(x => _consumerMock.Object);
            services.AddTransient(x => _producerMock.Object);

            new Startup().ConfigureServices(services, _configuration);
            services.AddHostedService<Worker>();
        }

        private void GivenAConfiguration()
        {
            _configuration = TestConfigurationProvider.ConfigFromObject(_appSettings, "WorkService.AppSettings.CLI.json");
        }

        private void GivenAnAppSettings()
        {
            _appSettings = new WorkerAppSettings
            {
                ConnectionString = "InvalidConnectionString",
                WorkerServiceSettings = new WorkerServiceSettings
                {
                    NumOfConsumers = 1
                }
            };
        }
    }
}
