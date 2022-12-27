using Backend.Modules.Tenants.Application.Contracts;
using Backend.Modules.Tenants.Application.DomainEvents.TenantRegistered;
using Backend.Modules.Tenants.Application.Exceptions;
using Backend.Modules.Tenants.Domain.Common;
using Backend.Modules.Tenants.Domain.RegistrationAggregate;

namespace Backend.Modules.Tenants.Application.Commands;

public static class RegisterTenant
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
        private readonly ITenantRepository _tenants;
        private readonly IPublisher _publisher;

        public Handler(IRegistrationRepository repository, ITenantRepository tenants, IPublisher publisher)
        {
            _repository = repository;
            _tenants = tenants;
            _publisher = publisher;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var identifier = TenantIdentifier.CreateInstance(request.Identifier);
            var tenant = await _tenants.Get(identifier, cancellationToken);
            if (tenant != null)
            {
                throw new TenantIdentifierAlreadyExistsException(identifier);
            }

            var email = Email.CreateInstance(request.Email);
            var name = TenantName.CreateInstance(request.Name);

            var registration = Registration.Register(email, name, identifier);

            if (!string.IsNullOrWhiteSpace(request.OverrideToken))
            {
                registration.OverrideToken(request.OverrideToken);
            }

            await _repository.Insert(registration, cancellationToken);

            await _publisher.Publish(new Notification(registration.Id), cancellationToken);

            return Unit.Value;
        }
    }
}