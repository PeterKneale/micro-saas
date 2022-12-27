using Backend.Modules.Tenants.Application.Contracts;
using Backend.Modules.Tenants.Application.Exceptions;
using Backend.Modules.Tenants.Domain.Common;

namespace Backend.Modules.Tenants.Application.Commands;

public static class ClaimTenant
{
    public record Command(string Identifier, string Password, string Token) : IRequest;

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Identifier).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Token).NotEmpty().EmailAddress();
        }
    }

    internal class Handler : IRequestHandler<Command>
    {
        private readonly IRegistrationRepository _repository;
        private readonly IPublisher _publisher;

        public Handler(IRegistrationRepository repository, IPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var identifier = TenantIdentifier.CreateInstance(request.Identifier);
            var token = request.Token;

            var registrations = await _repository.Get(identifier, cancellationToken);
            var registration = registrations.SingleOrDefault(x => x.Token == token);
            if (registration == null)
            {
                throw new RegistrationNotFoundException(identifier.Value);
            }

            registration.Claim(token);

            await _repository.Update(registration, cancellationToken);

            await _publisher.Publish(new Notifications.TenantClaimed.Notification(registration.Id), cancellationToken);

            return Unit.Value;
        }
    }
}