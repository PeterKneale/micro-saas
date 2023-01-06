using Backend.Modules.Tenants.Application.Contracts;
using Backend.Modules.Tenants.Application.Exceptions;

namespace Backend.Modules.Tenants.Application.Queries;

public static class GetTenantByIdentifier
{
    public record Query(string Identifier) : IRequest<Result>;

    public record Result(Guid Id, string Name, string Identifier);

    internal class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Identifier).NotEmpty();
        }
    }

    internal class Handler : IRequestHandler<Query, Result>
    {
        private readonly ITenantRepository _repository;

        public Handler(ITenantRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var identifier = TenantIdentifier.CreateInstance(request.Identifier);

            var tenant = await _repository.Get(identifier, cancellationToken);
            if (tenant == null)
            {
                throw new TenantNotFoundException(request.Identifier);
            }

            return new Result(tenant.Id.Id, tenant.Name.Value, tenant.TenantIdentifier.Value);
        }
    }
}