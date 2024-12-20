using TennisBookings.Web.External;
using TennisBookings.Web.External.Models;

namespace TennisBookings.Web.IntegrationTests.Fakes;

public class FakeWithDataWeatherApiClient : IWeatherApiClient
{
    public const string Description = " Sunny during testing!";
    public Task<WeatherApiResult> GetWeatherForecastAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new WeatherApiResult
        {
            City = "Cracow",
            Weather = new WeatherCondition { Description = Description }
        });
    }
}