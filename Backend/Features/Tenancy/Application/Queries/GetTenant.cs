using Backend.Features.Tenancy.Domain.TenantAggregate;

namespace Backend.Features.Tenancy.Application.Queries;

public static class GetTenant
{
    public record Query(Guid Id) : IRequest<Result>;

    public record Result(Guid Id, string Name, string Identifier);

    internal class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
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
            var tenantId = TenantId.CreateInstance(request.Id);

            var tenant = await _repository.Get(tenantId, cancellationToken);
            if (tenant == null)
            {
                throw new TenantNotFoundException(request.Id);
            }

            return new Result(tenant.Id.Id, tenant.Name.Value, tenant.TenantIdentifier.Value);
        }
    }
}