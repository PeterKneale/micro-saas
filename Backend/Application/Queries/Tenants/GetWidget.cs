using Backend.Application.Contracts;
using Backend.Application.Contracts.Tenants;
using Backend.Application.Exceptions;
using Backend.Domain.WidgetAggregate;

namespace Backend.Application.Queries.Tenants;

public static class GetWidget
{
    public class Query : IRequest<Result>, IRequireTenantContext
    {
        public Query(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public record Result(Guid Id, string? Description);

    internal class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    internal class Handler : IRequestHandler<Query, Result>
    {
        private readonly IWidgetRepository _widgets;

        public Handler(IWidgetRepository cars)
        {
            _widgets = cars;
        }

        public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
        {
            var widgetId = WidgetId.CreateInstance(request.Id);

            var car = await _widgets.Get(widgetId, cancellationToken);
            if (car == null)
            {
                throw new WidgetNotFoundException(request.Id);
            }

            return new Result(car.Id.Id, car.Description?.Value);
        }
    }
}