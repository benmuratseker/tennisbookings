using Microsoft.AspNetCore.Mvc.Testing;

namespace TennisBookings.Merchandise.Api.IntegrationTests.Controllers;

public class CategoriesControllerTests: IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;

    public CategoriesControllerTests(WebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateDefaultClient();
    }

    [Fact]
    public async Task GetAll_ReturnSuccessStatusCode()
    {
        var response = await _client.GetAsync("/api/categories");
        
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAll_ReturnExpectedMediaType()
    {
        var response = await _client.GetAsync("/api/categories");
        
        Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
    }
    
    [Fact]
    public async Task ReturnsContent()
    {
        var response = await _client.GetAsync("/api/categories");
        
        Assert.True(response.Content.Headers.ContentLength > 0);
    }
    
    [Fact]
    public async Task GetAll_ReturnExpectedJson()
    {
        var response = await _client.GetStringAsync("/api/categories");
        
        Assert.Equal("{\"allowedCategories\":[\"Accessories\",\"Bags\",\"Balls\",\"Clothing\",\"Rackets\"]}", response);
    }
}