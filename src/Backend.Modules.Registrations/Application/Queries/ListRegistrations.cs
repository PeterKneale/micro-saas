using Backend.Modules.Registrations.Application.Contracts;

namespace Backend.Modules.Registrations.Application.Queries;

public static class ListRegistrations
{
    public record Query : IRequest<IEnumerable<Result>>;

    public record Result(Guid Id, string Email, string Name, string Identifier, DateTime RegisteredAt, DateTime? ClaimedAt);

    internal class Validator : AbstractValidator<Query>
    {
    }

    internal class Handler : IRequestHandler<Query, IEnumerable<Result>>
    {
        private readonly IRegistrationRepository _repository;

        public Handler(IRegistrationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result>> Handle(Query request, CancellationToken cancellationToken)
        {
            var items = await _repository.List(cancellationToken);

            var models = items.Select(x => new Result(x.Id.Id, x.Email.Value, x.Name.Value, x.Identifier.Value, x.RegisteredAt, x.ClaimedAt));

            return models;
        }
    }
}