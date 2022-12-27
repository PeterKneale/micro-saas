namespace Backend.Modules.Settings.Application.Queries;

public static class GetTheme
{
    public record Query : IRequest<string>, IRequireTenantContext;

    internal class Handler : IRequestHandler<Query, string>
    {
        private readonly ISettingsRepository _repository;

        public Handler(ISettingsRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(Query request, CancellationToken cancellationToken)
        {
            var settings = await _repository.Get(cancellationToken);
            if (settings == null)
            {
                settings = Domain.SettingsAggregate.Settings.Create();
                await _repository.Insert(settings, cancellationToken);
            }
            return settings.GetTheme();
        }
    }
}