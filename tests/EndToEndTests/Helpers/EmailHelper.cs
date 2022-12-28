using Mailhog;
using Mailhog.Models;

namespace EndToEndTests.Helpers;

public static class EmailHelper
{
    private static readonly Uri MailHogUri = new("http://localhost:8025", UriKind.Absolute);

    public static async Task<string> GetClaimLinkFromRegisteredEmail(string email)
    {
        var client = new MailhogClient(MailHogUri);
        var messages = await client.SearchAsync(SearchKind.To, email);
        var message =  messages.Items.Single(x => x.Subject == "Registered");
        await DeleteMessage(message);
        return GetLinkFromMessage(message);
    }

    private static async Task DeleteMessage(Message message)
    {
        var client = new MailhogClient(MailHogUri);
        await client.DeleteAsync(message.ID);
    }

    private static string GetLinkFromMessage(Message message) =>
        //x-cta-url: http://localhost:8030/claim?identifier=identifier-f51aef&token=e490f6bf-b6a5-4e24-8e9c-033b1530d6e7
        // hack
        message.Raw.Data[11..110];
}