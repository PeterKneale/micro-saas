using Backend.Application.Contracts;
using Backend.Application.Contracts.Admin;
using Backend.Application.Exceptions;
using Backend.Domain.TenantAggregate;

namespace Backend.Application.Queries.Admin;

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
        private readonly IManagementRepository _repository;

        public Handler(IManagementRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var identifier = Identifier.CreateInstance(request.Identifier);

            var tenant = await _repository.Get(identifier, cancellationToken);
            if (tenant == null)
            {
                throw new TenantNotFoundException(request.Identifier);
            }

            return new Result(tenant.Id.Id, tenant.Name.Value, tenant.Identifier.Value);
        }
    }
}