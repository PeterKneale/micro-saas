namespace Backend.Features.Tenancy.Application.Notifications.TenantCreated;

public record Notification(TenantId TenantId) : INotification;