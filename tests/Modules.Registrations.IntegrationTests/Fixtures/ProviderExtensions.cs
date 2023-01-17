using Modules.Infrastructure.Tenancy;

namespace Modules.Registrations.IntegrationTests.Fixtures;

public static class ProviderExtensions
{
    public static async Task ExecuteCommand(this IServiceProvider provider, IRequest command, Guid? tenant = null)
    {
        using var scope = provider.CreateScope();
        if (tenant != null)
        {
            SetTenantContext(tenant.Value, scope);
        }

        var module = scope.ServiceProvider.GetRequiredService<IRegistrationsModule>();
        await module.ExecuteCommandAsync(command);
    }

    public static async Task<T> ExecuteQuery<T>(this IServiceProvider provider, IRequest<T> query, Guid? tenant = null)
    {
        using var scope = provider.CreateScope();
        if (tenant != null)
        {
            SetTenantContext(tenant.Value, scope);
        }

        var module = scope.ServiceProvider.GetRequiredService<IRegistrationsModule>();
        return await module.ExecuteQueryAsync(query);
    }

    private static void SetTenantContext(Guid tenant, IServiceScope scope)
    {
        // resolve the tenant context so that it can be set for this use case
        var setter = scope.ServiceProvider.GetRequiredService<IExecutionContextAccessor>() as FakeExecutionContextAccessor;
        setter!.SetCurrentTenant(tenant);
    }
}