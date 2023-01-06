﻿using Microsoft.Extensions.DependencyInjection;

namespace Backend.Modules.Tenants;

public interface ITenantsModule
{
    Task ExecuteCommandAsync(IRequest command);

    Task<TResult> ExecuteQueryAsync<TResult>(IRequest<TResult> query);
}

public class TenantsModule : ITenantsModule
{
    public async Task ExecuteCommandAsync(IRequest command)
    {
        using var scope = TenantsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IRequest<TResult> query)
    {
        using var scope = TenantsCompositionRoot.BeginLifetimeScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(query);
    }
}