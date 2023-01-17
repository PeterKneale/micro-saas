using System.Net;
using System.Net.Mail;
using Modules.Application;

namespace Modules.Infrastructure.Emails;

internal class EmailSender : IEmailSender
{
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    private readonly string? _smtpUsername;
    private readonly string? _smtpPassword;

    public EmailSender(IConfiguration configuration)
    {
        _smtpHost = configuration.GetSmtpHost();
        _smtpPort = configuration.GetSmtpPort();
        _smtpUsername = configuration.GetSmtpUsername();
        _smtpPassword = configuration.GetSmtpPassword();
    }

    public async Task SendRegisteredEmail(string email, Uri claimUri, CancellationToken cancellationToken)
    {
        var message = new MailMessage
        {
            To = {new MailAddress(email)},
            Subject = "Registered",
            From = new MailAddress("DoNotReply@localhost.com"),
            Body = claimUri.ToString()
        };
        message.Headers.Add("x-cta-url", claimUri.ToString());
        await SendEmail(message, cancellationToken);
    }

    public async Task SendClaimedEmail(string email, Uri loginUri, CancellationToken cancellationToken)
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