using System.Reflection;
using Backend.Modules.Infrastructure.Configuration;
using Backend.Modules.Infrastructure.Database;
using Backend.Modules.Infrastructure.Tenancy;
using Backend.Modules.Statistics.Application.Contracts;
using Backend.Modules.Statistics.Infrastructure;
using Backend.Modules.Statistics.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Backend.Modules.Statistics;

public static class StatisticsModuleSetup
{
    private static ServiceProvider? _provider;

    public static void Init(IExecutionContextAccessor executionContextAccessor, ILoggerFactory logger, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        var services = new ServiceCollection();

        // passed from host 
        services.AddLogging();
        services.AddSingleton(executionContextAccessor);
        services.AddSingleton(logger);
        services.AddSingleton(configuration);
        
        // add core 
        services.AddModules(configuration);
            
        // application
        services.AddMediatR(assembly);
        services.AddValidatorsFromAssembly(assembly);

        // infrastructure
        services.AddScoped<ITenantStatisticsRepository, TenantStatisticsRepository>();
        
        _provider = services.BuildServiceProvider();

        StatisticsCompositionRoot.SetProvider(_provider);
    }
    
    public static void SetupDatabase(Action<MigrationExecutor> action)
    {
        var configuration = _provider.GetRequiredService<IConfiguration>();
        var connection = configuration.GetSystemConnectionString();
        MigrationRunner.Run(connection, action);
    }
}