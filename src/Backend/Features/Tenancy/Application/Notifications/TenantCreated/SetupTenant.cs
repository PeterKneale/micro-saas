using Backend.Features.Tenancy.Domain.SettingsAggregate;

namespace Backend.Features.Tenancy.Application.Notifications.TenantCreated;

public class SetupTenant
{
    internal class Handler : INotificationHandler<Notification>
    {
        private readonly ITenantRepository _tenants;
        private readonly ILogger<Handler> _logs;

        public Handler(ITenantRepository tenants, ILogger<Handler> logs)
        {
            _tenants = tenants;
            _logs = logs;
        }

        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            var tenant = await _tenants.Get(notification.TenantId, cancellationToken);
            if (tenant == null)
            {
                throw new RegistrationNotFoundException(notification.TenantId.Id);
            }

            _logs.LogInformation("Tenant created {TenantName}", tenant.Name);
        }
    }
}