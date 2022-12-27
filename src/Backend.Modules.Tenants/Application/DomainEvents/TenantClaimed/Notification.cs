using Backend.Modules.Tenants.Domain.RegistrationAggregate;

namespace Backend.Modules.Tenants.Application.DomainEvents.TenantClaimed;

public record Notification(RegistrationId TenantId) : INotification;