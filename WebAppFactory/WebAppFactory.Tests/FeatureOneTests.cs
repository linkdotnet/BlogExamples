using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace WebAppFactory.Tests;

public class FeatureOneTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;

    public FeatureOneTests(WebApplicationFactory<Program> factory)
    {
        factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
        });
        client = factory.CreateClient();
    }

    [Fact]
    public async Task PassingNameShouldReturnNiceWelcomeMessage()
    {
        var response = await client.PostAsJsonAsync("/", new
        {
            Name = "Steven"
        });

        Assert.True(response.IsSuccessStatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Hello Steven!", content);
    }
}