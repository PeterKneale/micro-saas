namespace Backend.Modules.Settings.Application.Contracts;

internal interface ISettingsRepository
{
    Task Insert(Domain.SettingsAggregate.Settings settings, CancellationToken cancellationToken);
    Task Update(Domain.SettingsAggregate.Settings settings, CancellationToken cancellationToken);
    
    Task<Domain.SettingsAggregate.Settings?> Get(CancellationToken cancellationToken);
}