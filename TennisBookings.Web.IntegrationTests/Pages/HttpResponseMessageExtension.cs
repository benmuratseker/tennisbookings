using System.Net;

namespace TennisBookings.Web.IntegrationTests.Pages;

public static class HttpResponseMessageExtension
{
    public static void AssertOk(this HttpResponseMessage response)
    {
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}