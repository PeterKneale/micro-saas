using Backend.Features.Tenancy.Application.Contracts;
using Backend.Features.Tenancy.Application.Exceptions;
using Backend.Features.Tenancy.Domain.TenantAggregate;
using Backend.Features.Tenancy.Domain.UserAggregate;

namespace Backend.Features.Tenancy.Application.Commands;

public static class RegisterTenant
{
    public record Command(string Name, string Email, string Identifier) : IRequest;

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Identifier).NotEmpty().MaximumLength(100);
        }
    }

    internal class Handler : IRequestHandler<Command>
    {
        private readonly ITenantRepository _repository;

        public Handler(ITenantRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var email = Email.CreateInstance(request.Email);
            var token = Guid.NewGuid().ToString();

            var name = TenantName.CreateInstance(request.Name);
            var identifier = Identifier.CreateInstance(request.Identifier);

            var tenantId = TenantId.CreateInstance();
            var exists = await _repository.Get(identifier, cancellationToken);
            if (exists != null)
            {
                throw new TenantIdentifierAlreadyExistsException(identifier);
            }

            var tenant = Tenant.Provision(tenantId, name, identifier);
            await _repository.Insert(tenant, cancellationToken);

            var userId = UserId.CreateInstance();
            var user = User.ProvisionAdministrator(tenantId, userId, email, token);

            return Unit.Value;
        }
    }
}