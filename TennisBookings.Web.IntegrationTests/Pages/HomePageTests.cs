using TennisBookings.Web.IntegrationTests.Helpers;

namespace TennisBookings.Web.IntegrationTests.Pages;

public class HomePageTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> _factory;

    public HomePageTests(CustomWebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }
//AngleSharp nuget used for page testing
    [Fact]
    public async Task Get_ReturnsPageWithExpectedH1()
    {
        var client = _factory.CreateClient();
        
        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();

        using var content = await HtmlHelpers.GetDocumentAsync(response);
        var h1 = content.QuerySelector("h1");
        Assert.Equal("Welcome to Tennis by the Sea!", h1.TextContent);
    }
}