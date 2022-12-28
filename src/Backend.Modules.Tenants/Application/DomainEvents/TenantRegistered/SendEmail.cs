using Backend.Modules.Application;
using Backend.Modules.Tenants.Application.Extensions;
using Microsoft.Extensions.Configuration;

namespace Backend.Modules.Tenants.Application.DomainEvents.TenantRegistered;

public class SendRegisteredEmail
{
    internal class Handler : INotificationHandler<Notification>
    {
        private readonly IRegistrationRepository _repository;
        private readonly IEmailSender _emails;
        private readonly IConfiguration _configuration;

        public Handler(IRegistrationRepository repository, IEmailSender emails, IConfiguration configuration)
        {
            _repository = repository;
            _emails = emails;
            _configuration = configuration;
        }

        public async Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            var registration = await _repository.Get(notification.RegistrationId, cancellationToken);
            if (registration == null)
            {
                throw new RegistrationNotFoundException(notification.RegistrationId.Id);
            }

            var email = registration.Email.Value;
            var identifier = registration.Identifier.Value;
            var token = registration.Token;

            var siteUri = _configuration.GetRegistrationSiteUri();
            var pathUri = new Uri($"/claim?identifier={identifier}&token={token}", UriKind.Relative);
            var link = new Uri(siteUri, pathUri);
            await _emails.SendRegisteredEmail(email, link, cancellationToken);
        }
    }
}