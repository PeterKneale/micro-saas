using Modules.Settings.Application.Contracts;

namespace Modules.Settings.Application.Commands;

public static class ResetTheme
{
    public record Command : IRequest, IRequireTenantContext;

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
                return Unit.Value;
            }
            
            settings.ResetTheme();
            await _repository.Update(settings, cancellationToken);

            return Unit.Value;
        }
    }
}