using Backend.Modules.Tenants.Domain.RegistrationAggregate;

namespace Backend.Modules.Tenants.Application.Notifications.TenantRegistered;

public record Notification(RegistrationId RegistrationId) : INotification;