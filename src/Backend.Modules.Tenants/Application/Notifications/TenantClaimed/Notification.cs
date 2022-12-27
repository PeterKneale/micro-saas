using Backend.Modules.Tenants.Domain.RegistrationAggregate;

namespace Backend.Modules.Tenants.Application.Notifications.TenantClaimed;

public record Notification(RegistrationId TenantId) : INotification;