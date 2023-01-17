using Modules.Statistics.Application.Contracts;

namespace Modules.Statistics.Application.Queries;

public static class GetStatistics
{
    public class Query : IRequest<Result>
    {
    }

    public record Result(int TotalTenants, int TotalWidgets);

    internal class Validator : AbstractValidator<Query>
    {
    }

    internal class Handler : IRequestHandler<Query, Result>
    {
        private readonly ITenantStatisticsRepository _repository;

        public Handler(ITenantStatisticsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var totalTenants = await _repository.CountTenants(cancellationToken);
            var totalWidgets = await _repository.CountWidgets(cancellationToken);

            return new Result(totalTenants, totalWidgets);
        }
    }
}