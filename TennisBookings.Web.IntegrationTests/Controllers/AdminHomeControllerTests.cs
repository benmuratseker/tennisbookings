using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.TestHost;
using TennisBookings.Web.IntegrationTests.Helpers;

namespace TennisBookings.Web.IntegrationTests.Controllers;

public class AdminHomeControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> _factory;

    public AdminHomeControllerTests(CustomWebApplicationFactory<Startup> factory)
    {
        factory.ClientOptions.AllowAutoRedirect = false;
        _factory = factory;
    }

    [Fact]
    public async Task Get_SecurePageIsForbiddenForUnAuthenticatedUser()
    {
        var client = _factory.CreateClient();
        
        var response = await client.GetAsync("/Admin");
        
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.StartsWith("http://localhost/identity/account/login", response.Headers.Location.OriginalString, StringComparison.OrdinalIgnoreCase);
        
    }

    public static IEnumerable<object[]> RoleAccess => new List<object[]>
    {
        new object[] { "Header", HttpStatusCode.Forbidden },
        new object[] { "Admin", HttpStatusCode.OK },
    };

    [Theory]
    [MemberData(nameof(RoleAccess))]
    public async Task Get_SecurePageAccessibleOnlyByAdminUsers(string role, HttpStatusCode expectedStatusCode)
    {
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddAuthentication("Test")
                    .AddScheme<TestAuthenticationSchemeOptions,
                        TestAuthenticationHandler>("Test",
                        options => options.Role = role);
            });
        }).CreateClient();
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        var response = await client.GetAsync("/Admin");
        
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }
}