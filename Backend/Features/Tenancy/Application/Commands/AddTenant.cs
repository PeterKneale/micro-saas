using Backend.Features.Tenancy.Application.Contracts;
using Backend.Features.Tenancy.Application.Exceptions;
using Backend.Features.Tenancy.Domain.TenantAggregate;

namespace Backend.Features.Tenancy.Application.Commands;

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

            var exists = await _repository.Get(tenantId, cancellationToken);
            if (exists != null)
            {
                throw new TenantAlreadyExistsException(request.Id);
            }

            var name = Name.CreateInstance(request.Name);
            var identifier = Identifier.CreateInstance(request.Identifier);
            var tenant = Tenant.CreateInstance(tenantId, name, identifier);

            await _repository.Insert(tenant, cancellationToken);

            return Unit.Value;
        }
    }
}