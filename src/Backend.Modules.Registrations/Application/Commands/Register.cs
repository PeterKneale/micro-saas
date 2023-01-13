using Backend.Modules.Registration.Messages;
using Backend.Modules.Registrations.Application.Contracts;
using Backend.Modules.Registrations.Application.IntegrationEvents;
using Backend.Modules.Registrations.Domain.Common;

namespace Backend.Modules.Registrations.Application.Commands;

public static class Register
{
    public record Command(string Email, string Name, string Identifier, string? OverrideToken = null) : IRequest;

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Identifier).NotEmpty().MaximumLength(100);
        }
    }

    internal class Handler : IRequestHandler<Command>
    {
        private readonly IRegistrationRepository _repository;
        private readonly ICapPublisher _publisher;

        public Handler(IRegistrationRepository repository, ICapPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var identifier = TenantIdentifier.CreateInstance(request.Identifier);

            var email = Email.CreateInstance(request.Email);
            var name = TenantName.CreateInstance(request.Name);

            var registration = Domain.RegistrationAggregate.Registration.Register(email, name, identifier);

            if (!string.IsNullOrWhiteSpace(request.OverrideToken))
            {
                registration.OverrideToken(request.OverrideToken);
            }

            await _repository.Insert(registration, cancellationToken);

            await _publisher.PublishAsync(Topics.TenantRegistered, new TenantRegisteredIntegrationEvent
            {
                RegistrationId = registration.Id.Id
            }, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}