using TennisBookings.Web.Services;

namespace TennisBookings.Web.IntegrationTests.Helpers;

public class FixedDateTime : IDateTime
{
    public static DateTime UtcNow => new DateTime(2024, 12, 8, 15, 35, 00);
    public DateTime DateTimeUtc => UtcNow;
}