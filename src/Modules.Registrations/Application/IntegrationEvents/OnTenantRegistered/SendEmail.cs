using Modules.Application;
using Modules.Registrations.Application.Extensions;
using Microsoft.Extensions.Configuration;
using Modules.Registrations.Application.Contracts;
using Modules.Registrations.Application.Exceptions;
using Modules.Registrations.Domain.RegistrationAggregate;
using Modules.Registrations.Messages;

namespace Modules.Registrations.Application.IntegrationEvents.OnTenantRegistered;

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

    [CapSubscribe(Topics.TenantRegistered)]
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
