using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Api.Kestrel.CLI.Tests
{
    public class ApiStubFramework
    {
        private int _nextPort = 4000;
        private ApiStubFramework() { }
        private static readonly Lazy<ApiStubFramework> _instance = new (() => new ApiStubFramework(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
        public static ApiStubFramework Instance => _instance.Value;

        public IApiStub CreateApiStub()
        {
            var baseUrl = $"http://localhost:{_nextPort++}";

            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls(baseUrl);

            var app = builder.Build();
            var apiStub = new ApiStub(app, new Uri(baseUrl));
            return apiStub;
        }
    }
}