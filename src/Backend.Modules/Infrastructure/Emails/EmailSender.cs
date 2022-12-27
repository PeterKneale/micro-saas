using System.Net;
using System.Net.Mail;

namespace Backend.Modules.Infrastructure.Emails;

internal class EmailSender : IEmailSender
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string? _smtpUsername;
    private readonly string? _smtpPassword;

    public EmailSender(IConfiguration configuration)
    {
        _smtpHost = configuration["SmtpHost"] ?? "localhost";
        _smtpPort = int.Parse(configuration["SmtpPort"] ?? "1025");
        _smtpUsername = configuration["SmtpUsername"];
        _smtpPassword = configuration["SmtpPassword"];
    }

    public async Task SendRegisteredEmail(string email, string claimUri, CancellationToken cancellationToken)
    {
        var message = new MailMessage
        {
            To = {new MailAddress(email)},
            Subject = "Registered",
            From = new MailAddress("DoNotReply@localhost.com"),
            Body = claimUri
        };
        message.Headers.Add("x-cta-url", claimUri);
        await SendEmail(message, cancellationToken);
    }

    public async Task SendClaimedEmail(string email, string loginUri, CancellationToken cancellationToken)
    {
        var message = new MailMessage
        {
            To = {new MailAddress(email)},
            Subject = "Claimed",
            From = new MailAddress("DoNotReply@localhost.com"),
            Body = loginUri.ToString()
        };
        message.Headers.Add("x-cta-url", loginUri.ToString());
        await SendEmail(message, cancellationToken);
    }

    private async Task SendEmail(MailMessage message, CancellationToken cancellationToken)
    {
        using var smtp = new SmtpClient();
        smtp.Host = _smtpHost;
        smtp.Port = _smtpPort;
        smtp.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
        await smtp.SendMailAsync(message, cancellationToken);
    }
}