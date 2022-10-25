using Backend.Application.Contracts.Admin;

namespace Backend.Application.Queries.Admin;

public static class GetDashboard
{
    public class Query : IRequest<Result>
    {
    }

    public record Result(int TotalTenants, int TotalCars);

    internal class Validator : AbstractValidator<Query>
    {
    }

    internal class Handler : IRequestHandler<Query, Result>
    {
        private readonly IDashboardRepository _repository;

        public Handler(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var totalTenants = await _repository.CountTenants(cancellationToken);
            var totalCars = await _repository.CountCars(cancellationToken);

            return new Result(totalTenants, totalCars);
        }
    }
}