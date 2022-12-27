﻿using Backend.Features.Tenancy.Domain.TenantAggregate;

namespace Backend.Features.Tenancy.Application.Notifications.TenantClaimed;

public class SendEmail
{
    internal class Handler : INotificationHandler<Notification>
    {
        private readonly IRegistrationRepository _registrations;
        private readonly ITenantRepository _tenants;
        private readonly IEmailSender _emails;
        private readonly IPublisher _publisher;

        public Handler(IRegistrationRepository registrations, ITenantRepository tenants, IEmailSender emails, IPublisher publisher)
        {
            _registrations = registrations;
            _tenants = tenants;
            _emails = emails;
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
            
            await _emails.SendClaimedEmail(registration, cancellationToken);
            
            await _publisher.Publish(new Notifications.TenantCreated.Notification(tenantId), cancellationToken);
        }
    }
}