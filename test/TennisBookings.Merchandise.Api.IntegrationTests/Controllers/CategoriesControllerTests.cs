using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TennisBookings.Merchandise.Api.IntegrationTests.Models;
using TennisBookings.Merchandise.Api.IntegrationTests.TestHelpers.Serialization;

namespace TennisBookings.Merchandise.Api.IntegrationTests.Controllers;

public class CategoriesControllerTests: IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;

    public CategoriesControllerTests(WebApplicationFactory<Startup> factory)
    {
        // _client = factory.CreateDefaultClient();
        
        //_client = factory.CreateDefaultClient(new Uri("http://localhost/api/categories"));
        
        // _client = factory.CreateClient(
        //     new WebApplicationFactoryClientOptions()
        //     {
        //         BaseAddress = new Uri("https://localhost/api/categories")
        //     });
        
        factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/categories");
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnSuccessStatusCode()
    {
        // var response = await _client.GetAsync("/api/categories");
        var response = await _client.GetAsync("");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAll_ReturnExpectedMediaType()
    {
        // var response = await _client.GetAsync("/api/categories");
        var response = await _client.GetAsync("");
        Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
    }
    
    [Fact]
    public async Task ReturnsContent()
    {
        // var response = await _client.GetAsync("/api/categories");
        var response = await _client.GetAsync("");
        Assert.True(response.Content.Headers.ContentLength > 0);
    }
    
    [Fact]
    public async Task GetAll_ReturnsExpectedJson()
    {
        #region if someone changes the order of the allowed categories this one fails
         
         var response = await _client.GetStringAsync("/api/categories");
                
         Assert.Equal("{\"allowedCategories\":[\"Accessories\",\"Bags\",\"Balls\",\"Clothing\",\"Rackets\"]}", response);
         
        #endregion

        #region GetStreamAsync gives error
        // var expected = new List<string>{"Accessories", "Bags", "Balls", "Clothing", "Rackets"};
        // // var responseStream = await _client.GetStreamAsync("/api/categories");
        // var responseStream = await _client.GetStreamAsync("");
        //
        // var model = await JsonSerializer.DeserializeAsync<ExpectedCategoriesModel>(responseStream, JsonSerializerHelper.DefaultSerialisationOptions);
        // Assert.NotNull(model?.AllowedCategories);
        // Assert.Equal(expected.OrderBy(s => s), model.AllowedCategories.OrderBy(s => s));
        #endregion
    }

    [Fact]
    public async Task GetAll_ReturnsExpectedResponse()
    {
        var expected = new List<string>(){"Accessories", "Bags", "Balls", "Clothing", "Rackets"};

        var model = await _client.GetFromJsonAsync<ExpectedCategoriesModel>("");

        Assert.NotNull(model);
        Assert.Equal(expected.OrderBy(c => c), model.AllowedCategories.OrderBy(c => c));
    }

    [Fact]
    public async Task GetAll_SetsExpectedCacheControlHeader()
    {
        var response = await _client.GetAsync("");

        var header = response.Headers.CacheControl;
        
        Assert.True(header?.MaxAge.HasValue);
        Assert.Equal(TimeSpan.FromMinutes(5), header?.MaxAge);
        Assert.True(header?.Public);
    }
}