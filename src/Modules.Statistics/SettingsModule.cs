using Microsoft.Extensions.DependencyInjection;

namespace Modules.Statistics;

public interface IStatisticsModule
{
    Task ExecuteCommandAsync(IRequest command);

    Task<TResult> ExecuteQueryAsync<TResult>(IRequest<TResult> query);
}

public class StatisticsModule : IStatisticsModule
{
    public async Task ExecuteCommandAsync(IRequest command)
    {
        using var scope = StatisticsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IRequest<TResult> query)
    {
        using var scope = StatisticsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(query);
    }
}