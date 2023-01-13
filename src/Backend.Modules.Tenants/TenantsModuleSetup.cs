using System.Reflection;
using Backend.Modules.Infrastructure.Configuration;
using Backend.Modules.Infrastructure.Database;
using Backend.Modules.Infrastructure.Tenancy;
using Backend.Modules.Tenants.Infrastructure;
using Backend.Modules.Tenants.Infrastructure.Database;
using Backend.Modules.Tenants.Infrastructure.Repositories;
using DotNetCore.CAP.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Modules.Tenants;

public static class TenantsModuleSetup
{
    private static ServiceProvider? _provider;

    public static void Init(IExecutionContextAccessor context, ILoggerFactory logger, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        var services = new ServiceCollection();

        // passed from host 
        services.AddLogging();
        services.AddSingleton(context);
        services.AddSingleton(logger);
        services.AddSingleton(configuration);
        
        // add core 
        services.AddModules(configuration);
            
        // application
        services.AddMediatR(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services
            .AddScoped<Application.IntegrationEvents.OnTenantClaimed.CreateTenant>();

        // infrastructure
        services
            .AddScoped<ITenantRepository, TenantRepository>();
        
        _provider = services.BuildServiceProvider();

        TenantsCompositionRoot.SetProvider(_provider);
    }
    
    public static void SetupDatabase(Action<MigrationExecutor> action)
    {
        var configuration = _provider.GetRequiredService<IConfiguration>();
        var connection = configuration.GetSystemConnectionString();
        MigrationRunner.Run(connection, action);
    }

    public static void SetupOutbox()
    {
        _provider.GetRequiredService<IBootstrapper>()
            .BootstrapAsync()
            .GetAwaiter()
            .GetResult();
    }
}