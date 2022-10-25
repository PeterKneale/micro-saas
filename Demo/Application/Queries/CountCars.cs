using Demo.Application.Contracts;

namespace Demo.Application.Queries;

public static class CountCars
{
    public class Query : IRequest<Result>
    {
    }

    public record Result(int Total);

    internal class Validator : AbstractValidator<Query>
    {
    }

    internal class Handler : IRequestHandler<Query, Result>
    {
        private readonly IAdminRepository _repository;

        public Handler(IAdminRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var total = await _repository.Count(cancellationToken);

            return new Result(total);
        }
    }
}