using AngleSharp.Dom;
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

    public static IEnumerable<object[]> ConfigVariations => new List<object[]>
    {
        //global, page, should show
        new object[] { false, false, false },
        new object[] { true, false, false },
        new object[] { false, true, false },
        new object[] { true, true, true },
    };

    [Theory]
    [MemberData(nameof(ConfigVariations))]
    public async Task HomePageIncludesForecast_ForExpectedConfigVariations(
        bool globalEnabled, bool pagesEnabled, bool shouldShow)
    {
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((ctx, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    {
                        "Features:WeatherForecasting:EnableWeatherForecast", globalEnabled.ToString()
                    },
                    {
                        "Features:HomePage:EnableWeatherForecast", pagesEnabled.ToString()
                    },
                }!);
            });
        }).CreateClient();

        var respose = await client.GetAsync("/");
        using var content = await HtmlHelpers.GetDocumentAsync(respose);
        
        var forecastDiv = content.All.SingleOrDefault(e => e.Id == "weather-forecast" && e.LocalName == TagNames.Div);
        Assert.Equal(shouldShow, forecastDiv != null);
    }
}