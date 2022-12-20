using Backend.Features.Tenancy.Domain.RegistrationAggregate;
using Backend.Features.Tenancy.Domain.TenantAggregate;

namespace Backend.Features.Tenancy.Application.Notifications;

public class TenantClaimed
{
    public record Notification(RegistrationId RegistrationId) : INotification;

    internal class Handler : INotificationHandler<Notification>
    {
        private readonly IRegistrationRepository _registrations;
        private readonly ITenantRepository _tenants;
        private readonly IEmailSender _emails;

        public Handler(IRegistrationRepository registrations, ITenantRepository tenants, IEmailSender emails)
        {
            _registrations = registrations;
            _tenants = tenants;
            _emails = emails;
        }

        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            var registration = await _registrations.Get(notification.RegistrationId, cancellationToken);
            if (registration == null)
            {
                throw new RegistrationNotFoundException(notification.RegistrationId.Id);
            }

            var tenantId = TenantId.CreateInstance();
            var name = registration.Name;
            var identifier = registration.Identifier;
            var tenant = Tenant.Provision(tenantId, name, identifier);
            await _tenants.Insert(tenant, cancellationToken);
            
            await _emails.SendClaimedEmail(registration, cancellationToken);
        }
    }
}