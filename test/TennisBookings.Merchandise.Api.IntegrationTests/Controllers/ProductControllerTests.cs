using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using TennisBookings.Merchandise.Api.Data;
using TennisBookings.Merchandise.Api.Data.Dto;
using TennisBookings.Merchandise.Api.External.Database;
using TennisBookings.Merchandise.Api.IntegrationTests.Fakes;
using TennisBookings.Merchandise.Api.IntegrationTests.Models;
using TennisBookings.Merchandise.Api.IntegrationTests.TestHelpers.Serialization;

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

    [Fact]
    public async Task Post_WithoutName_ReturnsBadRequest()
    {
        var productInputModel = GetValidProuctInputModel().CloneWith(m => m.Name = null);
        
        var response = await _client.PostAsJsonAsync($"", productInputModel, JsonSerializerHelper.DefaultSerialisationOptions);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_InvalidName_ReturnsExpectedProblemDetails()
    {
        var productInputModel = GetValidProuctInputModel().CloneWith(m => m.Name = null);
        
        var response = await _client.PostAsJsonAsync($"", productInputModel, JsonSerializerHelper.DefaultSerialisationOptions);
        
        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        
        Assert.Collection(problemDetails?.Errors, kvp =>
        {
            Assert.Equal("Name", kvp.Key);
            var error = Assert.Single(kvp.Value);
            Assert.Equal("The Name field is required.", error);
        });
    }
    public static TestProductInputModel GetValidProuctInputModel(Guid? id = null)
    {
        return new TestProductInputModel
        {
            Id = id is object ? id.Value.ToString() : Guid.NewGuid().ToString(),
            Name = "Some Product",
            Description = "Some Description",
            Category = new CategoryProvider().AllowedCategories().First(),
            InternalReference = "ABC123",
            Price = 4.00m
        };
    }
}