using System.Net;

namespace TennisBookings.Web.IntegrationTests;

public class AuthenticationTests :IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> _factory;

    public AuthenticationTests(CustomWebApplicationFactory<Startup> factory)
    {
        factory.ClientOptions.AllowAutoRedirect = false;
        _factory = factory;
    }

    [Theory]
    [InlineData("/Admin")]
    [InlineData("/Admin/Staff/Add")]
    [InlineData("/Admin/Courts/Bookings/Upcoming")]
    [InlineData("/Admin/Courts/Booking/1/Cancel")]
    [InlineData("/Admin/Courts/Maintenance/Upcoming")]
    [InlineData("/FindAvailableCourts")]
    [InlineData("/BookCourt/1")]
    [InlineData("/Bookings")]
    public async Task Get_SecurePageRedirectsAnUnauthenticatedUser(string url)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);
        
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.StartsWith("http://localhost/identity/account/login", response.Headers.Location.OriginalString, StringComparison.OrdinalIgnoreCase);
    }
}