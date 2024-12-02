using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace TennisBookings.Merchandise.Api.IntegrationTests;

public class HealthCheckTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _httpClient;
    public HealthCheckTests(WebApplicationFactory<Startup> factory)
    {
        _httpClient = factory.CreateDefaultClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsOk()
    {
        var response = await _httpClient.GetAsync("/healthcheck");

        response.EnsureSuccessStatusCode();
        
        //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}