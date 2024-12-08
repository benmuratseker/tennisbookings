using System.Net.Http.Headers;
using TennisBookings.Web.Data;

namespace TennisBookings.Web.IntegrationTests.Pages;

public static class WebApplicationFactoryExtensions
{
    public static HttpClient CreateClientWithMemberAndDbSetup(
        this CustomWebApplicationFactory<Startup> factory, Action<TennisBookingDbContext> configure)
    {
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.WithMemberUser().ConfigureTestDatabase(db =>
            {
                configure(db);
            });
        }).CreateClient();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Test");
        
        return client;
    }
}