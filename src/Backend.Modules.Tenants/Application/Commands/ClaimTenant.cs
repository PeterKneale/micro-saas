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
        private readonly ICapPublisher _publisher;

        public Handler(IRegistrationRepository repository, ICapPublisher publisher)
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

            var message = new TenantClaimedIntegrationEvent {RegistrationId = registration.Id.Id};
            await _publisher.PublishAsync(Topics.TenantClaimed, message, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}