using Backend.Infrastructure.Tenancy;

namespace Backend.IntegrationTests.Fixtures;

public static class ProviderExtensions
{
    public static async Task ExecuteCommand(this IServiceProvider provider, IRequest command, string? tenant = null)
    {
        using var scope = provider.CreateScope();
        if (tenant != null)
        {
            SetTenantContext(tenant, scope);
        }
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command);
    }

    public static async Task<T> ExecuteQuery<T>(this IServiceProvider provider, IRequest<T> query, string? tenant = null)
    {
        using var scope = provider.CreateScope();
        if (tenant != null)
        {
            SetTenantContext(tenant, scope);
        }
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(query);
    }

    private static void SetTenantContext(string tenant, IServiceScope scope)
    {
        // resolve the tenant context so that it can be set for this use case
        var setter = scope.ServiceProvider.GetRequiredService<ISetTenantContext>();
        setter.SetCurrentTenant(tenant);
    }
}