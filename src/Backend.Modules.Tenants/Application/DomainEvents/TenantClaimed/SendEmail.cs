using Backend.Modules.Application;
using Backend.Modules.Tenants.Application.DomainEvents.TenantRegistered;
using Backend.Modules.Tenants.Application.Extensions;
using Backend.Modules.Tenants.Domain.Common;
using Backend.Modules.Tenants.Domain.TenantAggregate;
using Microsoft.Extensions.Configuration;

namespace Backend.Modules.Tenants.Application.DomainEvents.TenantClaimed;

public class SendEmail
{
    internal class Handler : INotificationHandler<Notification>
    {
        private readonly IRegistrationRepository _registrations;
        private readonly ITenantRepository _tenants;
        private readonly IEmailSender _emails;
        private readonly IConfiguration _configuration;
        private readonly IPublisher _publisher;

        public Handler(IRegistrationRepository registrations, ITenantRepository tenants, IEmailSender emails, IConfiguration configuration, IPublisher publisher)
        {
            _registrations = registrations;
            _tenants = tenants;
            _emails = emails;
            _configuration = configuration;
            _publisher = publisher;
        }

        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            var registration = await _registrations.Get(notification.TenantId, cancellationToken);
            if (registration == null)
            {
                throw new RegistrationNotFoundException(notification.TenantId.Id);
            }

            var tenantId = TenantId.CreateInstance();
            var name = registration.Name;
            var identifier = registration.Identifier;
            var tenant = Tenant.Provision(tenantId, name, identifier);
            await _tenants.Insert(tenant, cancellationToken);

            var email = registration.Email.Value;
            var link = _configuration.GetFrontendSiteUri();
            await _emails.SendClaimedEmail(email, link, cancellationToken);

            await _publisher.Publish(new TenantCreated.Notification(tenantId), cancellationToken);
        }
    }
}