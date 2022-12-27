using Backend.Features.Tenancy.Domain.RegistrationAggregate;

namespace Backend.Features.Tenancy.Application.Notifications.TenantClaimed;

public record Notification(RegistrationId TenantId) : INotification;