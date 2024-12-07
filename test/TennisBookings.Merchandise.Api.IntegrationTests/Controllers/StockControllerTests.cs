using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using TennisBookings.Merchandise.Api.Data.Dto;
using TennisBookings.Merchandise.Api.External.Database;
using TennisBookings.Merchandise.Api.IntegrationTests.Fakes;
using TennisBookings.Merchandise.Api.IntegrationTests.Models;

namespace TennisBookings.Merchandise.Api.IntegrationTests.Controllers;

public class
    StockControllerTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Startup> _factory;

    public StockControllerTests(WebApplicationFactory<Startup> factory)
    {
        factory.ClientOptions.BaseAddress =
            new Uri("http://localhost/api/stock/");
        _client = factory.CreateClient();
        _factory = factory;
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

        // Assert.Equal("{\"stockItemTotal\":100}", response);
        Assert.Equal("{\"stockItemTotal\":1870}", response);
    }

    [Fact]
    public async Task GetAll_ReturnsExpectedJsonContentType()
    {
        var response = await _client.GetAsync("total");

        Assert.Equal("application/json",
            response?.Content?.Headers?.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetStockTotal_ReturnsExpectedJson()
    {
        var model =
            await _client.GetFromJsonAsync<ExpectedStockTotalOutputModel>(
                "total");

        Assert.NotNull(model);
        Assert.True(model.StockItemTotal > 0);
    }

    [Fact]
    public async Task GetStockTotal_ReturnsExpectedStockQuantity()
    {
        var cloudDatabase = new FakeCloudDatabase(new[]
        {
            new ProductDto { StockCount = 200 },
            new ProductDto { StockCount = 500 },
            new ProductDto { StockCount = 300 }
        });

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<ICloudDatabase>(cloudDatabase);
            });
        }).CreateClient();

        var model =
            await client.GetFromJsonAsync<ExpectedStockTotalOutputModel>(
                "total");

        Assert.Equal(1000, model?.StockItemTotal);
    }
}