﻿using Modules.Settings.Application.Contracts;

namespace Modules.Settings.Application.Commands;

public static class SetTheme
{
    public record Command(string Theme) : IRequest, IRequireTenantContext;

    internal class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Theme).NotEmpty();
        }
    }

    internal class Handler : IRequestHandler<Command>
    {
        private readonly ISettingsRepository _repository;

        public Handler(ISettingsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var settings = await _repository.Get(cancellationToken);
            if (settings == null)
            {
                settings = Domain.SettingsAggregate.Settings.Create();
                settings.SetTheme(request.Theme);
                await _repository.Insert(settings, cancellationToken);
            }
            else
            {
                settings.SetTheme(request.Theme);
                await _repository.Update(settings, cancellationToken);
            }

            return Unit.Value;
        }
    }
}