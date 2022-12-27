﻿namespace Backend.Features.Tenancy.Application.Notifications.TenantRegistered;

public class SendRegisteredEmail
{
    internal class Handler : INotificationHandler<Notification>
    {
        private readonly IRegistrationRepository _repository;
        private readonly IEmailSender _emails;

        public Handler(IRegistrationRepository repository, IEmailSender emails)
        {
            _repository = repository;
            _emails = emails;
        }

        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            var registration = await _repository.Get(notification.RegistrationId, cancellationToken);
            if (registration == null)
            {
                throw new RegistrationNotFoundException(notification.RegistrationId.Id);
            }

            await _emails.SendRegisteredEmail(registration, cancellationToken);
        }
    }
}