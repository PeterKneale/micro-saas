using Modules.Application;
using Modules.Registrations.Application.Extensions;
using Modules.Tenants.Messages;
using Microsoft.Extensions.Configuration;
using Modules.Registrations.Application.Contracts;
using Modules.Registrations.Application.Exceptions;
using Modules.Registrations.Domain.RegistrationAggregate;

namespace Modules.Registrations.Application.IntegrationEvents.OnTenantReady;

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

    [CapSubscribe(Topics.TenantReady)]
    public async Task Handle(TenantReadyIntegrationEvent message, CancellationToken cancellationToken)
    {
        var registrationId = RegistrationId.CreateInstance(message.RegistrationId);

        var registration = await _registrations.Get(registrationId, cancellationToken);
        if (registration == null)
        {
            throw new RegistrationNotFoundException(registrationId.Id);
        }

        var email = registration.Email.Value;
        var link = _configuration.GetFrontendSiteUri();
        await _emails.SendClaimedEmail(email, link, cancellationToken);
    }
}