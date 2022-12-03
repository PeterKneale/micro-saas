using Backend.Features.Widgets.Application.Contracts;
using Backend.Features.Widgets.Application.Exceptions;
using Backend.Features.Widgets.Domain.WidgetAggregate;

namespace Backend.Features.Widgets.Application.Commands;

public static class UpdateWidget
{
    public class Command : IRequest, IRequireTenantContext
    {
        public Command(Guid id, string description)
        {
            Id = id;
            Description = description;
        }

        public Guid Id { get; }
        public string Description { get; }
    }

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }

    internal class Handler : IRequestHandler<Command>
    {
        private readonly IWidgetRepository _repository;

        public Handler(IWidgetRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var widgetId = WidgetId.CreateInstance(request.Id);
            var description = Description.CreateInstance(request.Description);

            var car = await _repository.Get(widgetId, cancellationToken);
            if (car == null)
            {
                throw new WidgetNotFoundException(request.Description);
            }

            car.Update(description);

            await _repository.Update(car, cancellationToken);

            return Unit.Value;
        }
    }
}