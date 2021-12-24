using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using Xunit;

namespace Api.Kestrel.CLI.Tests
{
    public class UnitTest1 : IDisposable
    {
        IApiStub _apiStub = ApiStubFramework.Instance.CreateApiStub();

        public void Dispose()
        {
            _apiStub.Dispose();
        }

        [Fact]
        public void ShouldSpinUpAKestrelServersWithTwoEndpoints()
        {
            TestObject testObject = new TestObject
            {
                Foo = "Yup",
                Bar = 420,
                Snafu = true,
                Snarf = new[] { "Standar Operating Procedure" }
            };

            _apiStub.AddResponse(HttpMethod.Get, "/staticString", HttpStatusCode.OK, "Hello World");
            _apiStub.AddResponse(HttpMethod.Get, "/jsonObject", HttpStatusCode.OK, testObject);
            _apiStub.StartAsync();
            
            var baseUrl = _apiStub.BaseUrl;
            Thread.Sleep(TimeSpan.FromMinutes(2));
        }
    }

    internal class TestObject
    {
        public string Foo { get; internal set; }
        public int Bar { get; internal set; }
        public bool Snafu { get; internal set; }
        public IEnumerable<string> Snarf { get; internal set; }
    }
}