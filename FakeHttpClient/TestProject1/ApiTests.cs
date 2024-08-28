using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject1;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(s =>
            s.ConfigureTestServices(sc =>
            {
                sc.AddTransient<FakeDataHandler>();
                sc.ConfigureHttpClientDefaults(d =>
                    d.AddHttpMessageHandler<FakeDataHandler>());
            }));
    }
    
    [Fact]
    public async Task ShouldGreet()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetStringAsync("/");

        Assert.NotNull(response);
        Assert.Equal("Hello World", response);
    }

    private class FakeDataHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri is { AbsoluteUri: "https://steven-giesel.com/api" })
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Hello World"),
                });
            }
            
            return base.SendAsync(request, cancellationToken);
        }
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}