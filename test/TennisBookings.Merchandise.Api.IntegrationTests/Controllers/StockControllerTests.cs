using Microsoft.AspNetCore.Mvc.Testing;
using TennisBookings.Merchandise.Api.IntegrationTests.Models;

namespace TennisBookings.Merchandise.Api.IntegrationTests.Controllers;

public class StockControllerTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;

    public StockControllerTests(WebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateDefaultClient(new Uri("http://localhost/api/stock/") );
    }

    [Fact]
    public async Task GetStockTotal_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync("total");
        
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAll_ReturnsExpectedJsonContentString()
    {
        var response = await _client.GetStringAsync("total");
        
        Assert.Equal("{\"stockItemTotal\":100}",response);
    }

    [Fact]
    public async Task GetAll_ReturnsExpectedJsonContentType()
    {
        var response = await _client.GetAsync("total");
        
        Assert.Equal("application/json", response?.Content?.Headers?.ContentType?.MediaType);
    }

    [Fact]
    public async Task Get_ReturnsExpectedJson()
    {
        var model = await _client.GetFromJsonAsync<ExpectedStockTotalOutputModel>("total");

        Assert.NotNull(model);
        Assert.True(model.StockItemTotal > 0);
    }

    [Fact]
    public async Task Get_ReturnsExpectedStockQuantity()
    {
        var model = await _client.GetFromJsonAsync<ExpectedStockTotalOutputModel>("total");
        
        Assert.Equal(1000, model?.StockItemTotal);
    }
}