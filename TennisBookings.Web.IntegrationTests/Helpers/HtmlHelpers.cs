using AngleSharp;
using AngleSharp.Dom;

namespace TennisBookings.Web.IntegrationTests.Helpers;

public class HtmlHelpers
{
    public static async Task<IDocument> GetDocumentAsync(
        HttpResponseMessage response)
    {
        var contentStream = await response.Content.ReadAsStreamAsync();

        var browser = new BrowsingContext();
        var document = await browser.OpenAsync(virtualResponse =>
        {
            virtualResponse.Content(contentStream, shouldDispose: true);
            virtualResponse.Address(response.RequestMessage.RequestUri).Status(response.StatusCode);
        });
        return document;
    }
}