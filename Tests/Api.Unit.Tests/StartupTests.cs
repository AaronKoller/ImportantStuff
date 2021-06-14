using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Api;
using Api.Models;
using Api.Models.Exceptions;
using TestInfrastructure;
using Xunit;

namespace ApiTests.Unit.Tests
{
    public class StartupTests
    {
        private IServiceCollection _serviceCollection = new ServiceCollection();
        private AppSettings _appSettings;
        private IConfigurationRoot _configuration;
        private ServiceDescriptor _foundServiceDescriptor;
        private Action _action;

        [Fact]
        public void ShouldContainService()
        {
            GivenAnAppSettings();
            GivenAConfiguration();

            WhenConfigureServiceIsCalled();

            ThenServiceCollectionContains<IService>();
            ThenFoundServiceDescriptionImplements<Service>();
            ThenFoundServiceLifeTimeIs(ServiceLifetime.Transient);
        }

        [Fact]
        public void ShouldThrowException()
        {
            GivenAnAppSettings();
            GivenAnAppSettingsPropertyIsModified();
            GivenAConfiguration();

            WhenConfigureServiceIsCalledAsAction();

            ThenShouldThrowException();
        }

        private void GivenAnAppSettingsPropertyIsModified()
        {
            _appSettings.ChildAppSetting.ThisIsAProperty = "throw";
        }

        private void ThenShouldThrowException()
        {
            _action.Should().Throw<ExampleException>().WithMessage("get out of here");
        }

        private void WhenConfigureServiceIsCalledAsAction()
        {
            _action = WhenConfigureServiceIsCalled;
        }

        private void ThenFoundServiceLifeTimeIs(ServiceLifetime serviceLifeTime)
        {
            _foundServiceDescriptor.Lifetime.Should().Be(serviceLifeTime);
        }

        private void ThenFoundServiceDescriptionImplements<TImplementationType>()
        {
            _foundServiceDescriptor.ImplementationType.Should().Be(typeof(TImplementationType));
        }

        //TODO KEEP THIS AS AN EXAMPLE
        private void ThenFoundServiceDescriptionImplementationInstanceIs<TImplementationType>()
        {
            _foundServiceDescriptor.ImplementationInstance.GetType().Should().Be(typeof(TImplementationType));
        }

        private void ThenServiceCollectionContains<TInterfaceType>()
        {
            _foundServiceDescriptor = _serviceCollection.FirstOrDefault(sd => sd.ServiceType == typeof(TInterfaceType));
            _foundServiceDescriptor.Should().NotBeNull();
        }

        private void WhenConfigureServiceIsCalled()
        {
            var startup = new Startup(_configuration);

            startup.ConfigureServices(_serviceCollection);
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
