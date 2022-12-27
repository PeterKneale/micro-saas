namespace Backend.Modules.Tenants.Application.Queries;

public static class ListTenants
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(Guid Id, string Name, string Identifier);

    internal class Validator : AbstractValidator<Query>
    {
    }

    internal class Handler : IRequestHandler<Query, IEnumerable<Result>>
    {
        private readonly ITenantRepository _repository;

        public Handler(ITenantRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result>> Handle(Query request, CancellationToken cancellationToken)
        {
            var tenants = await _repository.ListTenants(cancellationToken);

            return tenants
                .Select(x => new Result(x.Id.Id, x.Name.Value, x.TenantIdentifier.Value));
        }
    }
}