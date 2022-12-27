using Backend.Features.Tenancy.Domain.RegistrationAggregate;

namespace Backend.Features.Tenancy.Application.Notifications.TenantRegistered;

public record Notification(RegistrationId RegistrationId) : INotification;