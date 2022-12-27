using Backend.Features.Tenancy.Domain.SettingsAggregate;

namespace Backend.Features.Tenancy.Application.Notifications.TenantCreated;

public class SetupTenant
{
    internal class Handler : INotificationHandler<Notification>
    {
        private readonly ISettingsRepository _settings;
        private readonly ITenantRepository _tenants;

        public Handler(ISettingsRepository settings, ITenantRepository tenants)
        {
            _settings = settings;
            _tenants = tenants;
        }

        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            var tenant = await _tenants.Get(notification.TenantId, cancellationToken);
            if (tenant == null)
            {
                throw new RegistrationNotFoundException(notification.TenantId.Id);
            }

            await _settings.Insert(Settings.Create(), cancellationToken);
        }
    }
}