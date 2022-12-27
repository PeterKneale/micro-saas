using Backend.Modules.Tenants.Domain.RegistrationAggregate;

namespace Backend.Modules.Tenants.Application.DomainEvents.TenantRegistered;

public record Notification(RegistrationId RegistrationId) : INotification;