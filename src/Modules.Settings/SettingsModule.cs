namespace Modules.Settings;

public interface ISettingsModule
{
    Task ExecuteCommandAsync(IRequest command);

    Task<TResult> ExecuteQueryAsync<TResult>(IRequest<TResult> query);
}

public class SettingsModule : ISettingsModule
{
    public async Task ExecuteCommandAsync(IRequest command)
    {
        using var scope = SettingsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IRequest<TResult> query)
    {
        using var scope = SettingsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(query);
    }
}