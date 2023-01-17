using System.Reflection;
using Modules;
using Modules.Infrastructure.Configuration;
using Modules.Infrastructure.Database;
using Modules.Infrastructure.Tenancy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modules.Statistics.Application.Contracts;
using Modules.Statistics.Infrastructure;
using Modules.Statistics.Infrastructure.Database;

namespace Modules.Statistics;

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