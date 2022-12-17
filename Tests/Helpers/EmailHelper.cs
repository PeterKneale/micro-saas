using System.Text.Encodings.Web;
using Mailhog;
using Mailhog.Models;

namespace Tests.Helpers;

public static class EmailHelper
{
    public static async Task<string> GetFirstClaimLink(string email)
    {
        var message = await GetSingleMessageTo(email);
        await DeleteMessage(message);
        return GetLinkFromMessage(message);
    }

    private static async Task<Message> GetSingleMessageTo(string email)
    {
        var client = new MailhogClient(new Uri("http://localhost:8025"));
        var messages = await client.SearchAsync(SearchKind.To, email);
        return GetFirstMessage(messages);
    }

    private static async Task DeleteMessage(Message message)
    {
        var client = new MailhogClient(new Uri("http://localhost:8025"));
        await client.DeleteAsync(message.ID);
    }

    private static string GetLinkFromMessage(Message message) =>
        //x-cta-url: http://localhost:8030/claim?identifier=identifier-f51aef&token=e490f6bf-b6a5-4e24-8e9c-033b1530d6e7
        // hack
        message.Raw.Data[11..110];
    
    private static Message GetFirstMessage(Messages messages)
    {
        if (!messages.Items.Any())
        {
            throw new Exception("No messages found");
        }
        return messages.Items.First();
    }
}