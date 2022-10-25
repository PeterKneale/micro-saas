using Backend.Application.Contracts;
using Backend.Application.Contracts.Admin;

namespace Backend.Application.Queries.Admin;

public static class ListTenants
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(Guid Id, string Name);

    internal class Validator : AbstractValidator<Query>
    {
    }

    internal class Handler : IRequestHandler<Query, IEnumerable<Result>>
    {
        private readonly IManagementRepository _repository;

        public Handler(IManagementRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result>> Handle(Query request, CancellationToken cancellationToken)
        {
            var cars = await _repository.ListTenants(cancellationToken);

            return cars
                .Select(x => new Result(x.Id.Id, x.Name.Value));
        }
    }
}