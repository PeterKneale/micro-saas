using Modules.Application;
using Modules.Widgets.Application.Contracts;

namespace Modules.Widgets.Application.Queries;

public static class ListWidgets
{
    public record Query : IRequest<IEnumerable<Result>>, IRequireTenantContext;

    public record Result(Guid Id, string? Description);

    internal class Validator : AbstractValidator<Query>
    {
    }

    internal class Handler : IRequestHandler<Query, IEnumerable<Result>>
    {
        private readonly IWidgetRepository _repository;

        public Handler(IWidgetRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Result>> Handle(Query request, CancellationToken cancellationToken)
        {
            var widgets = await _repository.List(cancellationToken);

            return widgets
                .Select(x => new Result(x.Id.Id, x.Description?.Value));
        }
    }
}