namespace Backend.Modules.Widgets;

public interface IWidgetsModule
{
    Task ExecuteCommandAsync(IRequest command);

    Task<TResult> ExecuteQueryAsync<TResult>(IRequest<TResult> query);
}

public class WidgetsModule : IWidgetsModule
{
    public async Task ExecuteCommandAsync(IRequest command)
    {
        using var scope = WidgetsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IRequest<TResult> query)
    {
        using var scope = WidgetsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(query);
    }
}