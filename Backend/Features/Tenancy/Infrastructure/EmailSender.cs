using System.Net;
using System.Net.Mail;
using Backend.Features.Tenancy.Domain.RegistrationAggregate;

namespace Backend.Features.Tenancy.Infrastructure;

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

    public async Task SendRegisteredEmail(Registration registration, CancellationToken cancellationToken)
    {
        var claimUri = BuildClaimUri(registration);
        var emailAddress = registration.Email.Value;
        var message = new MailMessage
        {
            To = {new MailAddress(emailAddress)},
            Subject = "Registered",
            From = new MailAddress("DoNotReply@localhost.com"),
            Body = claimUri.ToString()
        };
        message.Headers.Add("x-cta-url", claimUri.ToString());
        await SendEmail(message, cancellationToken);
    }

    public async Task SendClaimedEmail(Registration registration, CancellationToken cancellationToken)
    {
        var loginUri = BuildClaimUri(registration);
        var emailAddress = registration.Email.Value;
        var message = new MailMessage
        {
            To = {new MailAddress(emailAddress)},
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

    private static Uri BuildClaimUri(Registration registration)
    {
        var rootUri = new Uri("http://localhost:8030/", UriKind.Absolute);
        var relativeUri = new Uri($"/claim?identifier={registration.Identifier.Value}&token={registration.Token}", UriKind.Relative);
        return new Uri(rootUri, relativeUri);
    }
    
    private static Uri BuildLoginUri(Registration registration)
    {
        // TODO build login uri with hostname
        return new Uri("http://localhost:8010/", UriKind.Absolute);
    }
}