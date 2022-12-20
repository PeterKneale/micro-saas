using Backend.Features.Tenancy.Domain.RegistrationAggregate;

namespace Backend.Features.Tenancy.Application.Notifications;

internal interface IEmailSender
{
    Task SendRegisteredEmail(Registration registration, CancellationToken cancellationToken);
    Task SendClaimedEmail(Registration registration, CancellationToken cancellationToken);
}