using Microsoft.AspNetCore.TestHost;
using TennisBookings.Merchandise.Api.Data.Dto;
using TennisBookings.Merchandise.Api.External.Database;
using TennisBookings.Merchandise.Api.IntegrationTests.Fakes;
using TennisBookings.Merchandise.Api.IntegrationTests.Models;

namespace TennisBookings.Merchandise.Api.IntegrationTests.Controllers;

public class
    ProductControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Startup> _factory;

    public ProductControllerTests(CustomWebApplicationFactory<Startup> factory)
    {
        factory.ClientOptions.BaseAddress =
            new Uri("http://localhost/api/products/");
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task Get_All_ReturnsExpectedArrayOfProducts()
    {
        #region MyRegion

        // var cloudDatabase = new FakeCloudDatabase(new[]
        // {
        //     new ProductDto { StockCount = 200 },
        //     new ProductDto { StockCount = 500 },
        //     new ProductDto { StockCount = 300 }
        // });
        //
        // var client = _factory.WithWebHostBuilder(builder =>
        // {
        //     builder.ConfigureTestServices(services =>
        //     {
        //         services.AddSingleton<ICloudDatabase>(cloudDatabase);
        //     });
        // }).CreateClient();

        #endregion

        _factory.FakeCloudDatabase.ResetDefaultProducts(useCustomIfAvailable: false);
        var products =
            await _client.GetFromJsonAsync<ExpectedProductModel[]>("");

        Assert.NotNull(products);
        // Assert.Equal(5, products.Length);
        Assert.Equal(_factory.FakeCloudDatabase.Products.Count,
            products.Length);
    }

    [Fact]
    public async Task Get_ReturnsExpectedProduct()
    {
        #region MyRegion

        // var expectedId = Guid.NewGuid();
        //
        // var cloudDatabase = new FakeCloudDatabase(new[]
        // {
        //     new ProductDto { Id = expectedId, Name = "EXPECTED" },
        //     new ProductDto { Id = Guid.NewGuid(), Name = "NOT_EXPECTED_!" },
        //     new ProductDto { Id = Guid.NewGuid(), Name = "NOT_EXPECTED_2" },
        // });
        //
        // var client = _factory.WithWebHostBuilder(builder =>
        // {
        //     builder.ConfigureTestServices(services =>
        //     {
        //         services.AddSingleton<ICloudDatabase>(cloudDatabase);
        //     });
        // }).CreateClient();

        #endregion

        var firstProduct =_factory.FakeCloudDatabase.Products.First();
        var product = await _client.GetFromJsonAsync<ExpectedProductModel>(
            $"{firstProduct.Id}");
        
        Assert.NotNull(product);
        Assert.Equal(firstProduct.Name, product.Name);
    }
}