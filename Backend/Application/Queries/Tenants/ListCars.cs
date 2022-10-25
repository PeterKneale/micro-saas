using Backend.Application.Contracts;
using Backend.Application.Contracts.Tenants;

namespace Backend.Application.Queries.Tenants;

public static class ListCars
{
    public record Query : IRequest<IEnumerable<Result>>, IRequireTenantContext;

    public record Result(Guid Id, string? Registration);

    internal class Validator : AbstractValidator<Query>
    {
    }

    internal class Handler : IRequestHandler<Query, IEnumerable<Result>>
    {
        private readonly ICarRepository _cars;

        public Handler(ICarRepository cars)
        {
            _cars = cars;
        }

        public async Task<IEnumerable<Result>> Handle(Query request, CancellationToken cancellationToken)
        {
            var cars = await _cars.List(cancellationToken);

            return cars
                .Select(x => new Result(x.Id.Id, x.Registration?.RegistrationNumber));
        }
    }
}