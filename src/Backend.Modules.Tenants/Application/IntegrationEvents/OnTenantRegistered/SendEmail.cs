using Backend.Modules.Application;
using Backend.Modules.Tenants.Application.Extensions;
using Backend.Modules.Tenants.Domain.RegistrationAggregate;
using Backend.Modules.Tenants.Messages;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;

namespace Backend.Modules.Tenants.Application.IntegrationEvents.OnTenantRegistered;

internal class SendEmail : ICapSubscribe
{
    private readonly IRegistrationRepository _registrations;
    private readonly IEmailSender _emails;
    private readonly IConfiguration _configuration;

    public SendEmail(IRegistrationRepository registrations, IEmailSender emails, IConfiguration configuration)
    {
        _registrations = registrations;
        _emails = emails;
        _configuration = configuration;
    }

    [CapSubscribe("tenant-registered", Group = "send-emails")]
    public async Task Handle(TenantClaimedIntegrationEvent message, CancellationToken cancellationToken)
    {
        var registrationId = RegistrationId.CreateInstance(message.RegistrationId);
        
        var registration = await _registrations.Get(registrationId, cancellationToken);
        if (registration == null)
        {
            throw new RegistrationNotFoundException(registrationId.Id);
        }

        var email = registration.Email.Value;
        var identifier = registration.Identifier.Value;
        var token = registration.Token;

        var siteUri = _configuration.GetRegistrationSiteUri();
        var pathUri = new Uri($"/claim?identifier={identifier}&token={token}", UriKind.Relative);
        var link = new Uri(siteUri, pathUri);
        await _emails.SendRegisteredEmail(email, link, cancellationToken);
    }
}
