using Backend.Modules.Tenants.Domain.Common;

namespace Backend.Modules.Tenants.Application.Notifications.TenantCreated;

public record Notification(TenantId TenantId) : INotification;