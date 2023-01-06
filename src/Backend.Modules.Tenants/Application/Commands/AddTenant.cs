using Backend.Modules.Tenants.Domain.TenantAggregate;

namespace Backend.Modules.Tenants.Application.Commands;

public static class AddTenant
{
    public record Command(Guid Id, string Name, string Identifier) : IRequest;

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Identifier).NotEmpty();
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
            var tenantId = TenantId.CreateInstance(request.Id);
            var name = TenantName.CreateInstance(request.Name);
            var identifier = TenantIdentifier.CreateInstance(request.Identifier);

            var idExists = await _repository.Get(tenantId, cancellationToken);
            if (idExists != null)
            {
                throw new TenantAlreadyExistsException(tenantId);
            }
            
            var identifierExists = await _repository.Get(identifier, cancellationToken);
            if (identifierExists != null)
            {
                throw new TenantIdentifierAlreadyExistsException(identifier);
            }

            var tenant = Tenant.Provision(tenantId, name, identifier);

            await _repository.Insert(tenant, cancellationToken);

            return Unit.Value;
        }
    }
}