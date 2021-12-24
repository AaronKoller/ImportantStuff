using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Kestrel.CLI.Tests
{
    public interface IApiStub: IDisposable, IAsyncDisposable
    {
        public Uri BaseUrl { get; init; }
        Task StartAsync();
        void AddResponse(HttpMethod httpMethod, string path, HttpStatusCode responseStatus, string responseBody);
        void AddResponse<T>(HttpMethod httpMethod, string path, HttpStatusCode responseStatus, T responseBody);
        void AddResponse(HttpMethod get, string v, HttpStatusCode oK, object testObject);
    }
    public class ApiStub : IApiStub
    {
        private readonly WebApplication _app;

        public Uri BaseUrl { get; init; }

        public ApiStub(WebApplication app, Uri baseUrl)
        {
            _app = app;
            BaseUrl = baseUrl;
        }

        public Task StartAsync()
        {
            return _app.StartAsync();
        }

        public void AddResponse(HttpMethod httpMethod, string path, HttpStatusCode responseStatus, string responseBody)
        {
            _app.MapMethods(path, new[] { httpMethod.Method }, (HttpContext httpContext) => 
            { 
                httpContext.Response.StatusCode = (int)responseStatus;
                httpContext.Response.WriteAsync(responseBody);
            });
        }

        public void AddResponse<T>(HttpMethod httpMethod, string path, HttpStatusCode responseStatus, T responseBody)
        {
            _app.MapMethods(path, new[] { httpMethod.Method }, (HttpContext httpContext) =>
            {
                httpContext.Response.StatusCode = (int)responseStatus;
                httpContext.Response.WriteAsync(JsonSerializer.Serialize(responseBody));
            });
        }

        public void Dispose()
        {
            _app.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            ((IDisposable)_app).Dispose();
        }

        public ValueTask DisposeAsync()
        {
            _app.StopAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            return _app.DisposeAsync();            
        }
    }
}