using Microsoft.AspNetCore.TestHost;
using TennisBookings.Web.Data;
using TennisBookings.Web.IntegrationTests.Helpers;

namespace TennisBookings.Web.IntegrationTests.Pages;

public static class WebHostBuilderExtensions
{
    public static IWebHostBuilder WithMemberUser(this IWebHostBuilder builder)
    {
        return builder.ConfigureTestServices(services =>
        {
            AuthenticationServiceCollectionExtensions.AddAuthentication(services, "Test")
                .AddScheme<TestAuthenticationSchemeOptions, TestAuthenticationHandler>("Test",
                    options => options.Role = "Member");
        });
    }

    public static IWebHostBuilder ConfigureTestDatabase(
        this IWebHostBuilder builder, Action<TennisBookingDbContext> configure)
    {
        return builder.ConfigureTestServices(services =>
        {
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices
                .GetRequiredService<TennisBookingDbContext>();
            var logger = scopedServices
                .GetRequiredService<ILogger<BookingPageTests>>();

            try
            {
                configure(db);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred setting up the database for the test. Error: {Message}",e.Message);
            }
        });
    }
}