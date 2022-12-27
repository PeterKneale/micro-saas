using Backend.Modules.Tenants.Domain.Common;

namespace Backend.Modules.Tenants.Application.DomainEvents.TenantCreated;

public record Notification(TenantId TenantId) : INotification;