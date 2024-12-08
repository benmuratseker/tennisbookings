using TennisBookings.Merchandise.Api.Diagnostics;
using TennisBookings.Merchandise.Api.IntegrationTests.Fakes;

namespace TennisBookings.Merchandise.Api.IntegrationTests.Middleware;

public class MetricMiddlewareTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Startup> _factory;
    
    public MetricMiddlewareTests(CustomWebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateDefaultClient();
        _factory = factory;
    }

    [Fact]
    public async Task RequestForPage_ResultsInExpectedMetrics()
    {
        const string userAgent = "SomeProduct/1.0";
        
        var request1 = new HttpRequestMessage(HttpMethod.Get, "/does-not-exist");
        request1.Headers.TryAddWithoutValidation("User-Agent", userAgent);
        var request2 = new HttpRequestMessage(HttpMethod.Get, "/healthcheck");
        request2.Headers.TryAddWithoutValidation("User-Agent", userAgent);
        
        await _client.SendAsync(request1);
        await _client.SendAsync(request2);

        var metricsRecorder = _factory.Services.GetRequiredService<IMetricRecorder>() as
                FakeMetricRecorder;

        var metric1 = metricsRecorder.Metrics.FirstOrDefault();
        var metric2 = metricsRecorder.Metrics.LastOrDefault();
        
        Assert.Equal("sending-response", metric1.Name);
        Assert.Equal(1, metric1.Increment);
        Assert.Equal("status_code:404",metric1.Tags[0]);
        Assert.Equal($"user_agent:{userAgent}",metric1.Tags[1]);
        
        Assert.Equal("sending-response", metric2.Name);
        Assert.Equal(1, metric2.Increment);
        Assert.Equal("status_code:200",metric2.Tags[0]);
        Assert.Equal($"user_agent:{userAgent}",metric2.Tags[1]);
    }
}