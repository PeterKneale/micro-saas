using System.Reflection;
using Backend.Modules.Infrastructure.Database;
using Backend.Modules.Infrastructure.Tenancy;
using Backend.Modules.Tenants.Infrastructure;
using DotNetCore.CAP.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Modules.Tenants;

public static class TenantsModuleSetup
{
    private static ServiceProvider? _provider;

    public static void Init(IExecutionContextAccessor executionContextAccessor, ILoggerFactory logger, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        var services = new ServiceCollection();

        // passed from host 
        services.AddSingleton(executionContextAccessor);
        services.AddSingleton(logger);
        services.AddSingleton(configuration);
        
        // add core 
        services.AddModules(configuration);
            
        // application
        services.AddMediatR(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services
            .AddScoped<Application.IntegrationEvents.OnTenantClaimed.CreateTenant>()
            .AddScoped<Application.IntegrationEvents.OnTenantClaimed.SendEmail>()
            .AddScoped<Application.IntegrationEvents.OnTenantRegistered.SendEmail>();

        // infrastructure
        services
            .AddScoped<IRegistrationRepository, RegistrationRepository>()
            .AddScoped<ITenantRepository, TenantRepository>();
        
        _provider = services.BuildServiceProvider();

        TenantsCompositionRoot.SetProvider(_provider);
    }
    
    public static void SetupDatabase(Action<MigrationExecutor> action)
    {
        using var scope = _provider.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<MigrationExecutor>();
        action(migrator);
    }

    public static void SetupOutbox()
    {
        _provider.GetRequiredService<IBootstrapper>()
            .BootstrapAsync()
            .GetAwaiter()
            .GetResult();
    }
}