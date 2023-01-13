using Microsoft.Extensions.DependencyInjection;

namespace Backend.Modules.Registrations;

public interface IRegistrationsModule
{
    Task ExecuteCommandAsync(IRequest command);

    Task<TResult> ExecuteQueryAsync<TResult>(IRequest<TResult> query);
}

public class RegistrationsModule : IRegistrationsModule
{
    public async Task ExecuteCommandAsync(IRequest command)
    {
        using var scope = RegistrationsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IRequest<TResult> query)
    {
        using var scope = RegistrationsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(query);
    }
}