﻿using Backend.Application.Contracts;
using Backend.Application.Contracts.Tenants;
using Backend.Application.Exceptions;
using Backend.Domain.WidgetAggregate;

namespace Backend.Application.Commands.Tenants;

public static class AddWidget
{
    public record Command(Guid Id, string Description) : IRequest, IRequireTenantContext;

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

            var exists = await _repository.Get(widgetId, cancellationToken);
            if (exists != null)
            {
                throw new WidgetAlreadyExistsException(request.Id);
            }

            var description =  Description.CreateInstance(request.Description);
            var widget = Widget.CreateInstance(widgetId, description);
            
            await _repository.Insert(widget, cancellationToken);

            return Unit.Value;
        }
    }
}