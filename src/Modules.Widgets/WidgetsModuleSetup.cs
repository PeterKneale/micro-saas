using Modules;
using Modules.Infrastructure.Configuration;
using Modules.Infrastructure.Database;
using Modules.Widgets.Infrastructure;
using Modules.Widgets.Application.Contracts;
using Modules.Widgets.Infrastructure.Database;
using Modules.Widgets.Infrastructure.Repositories;

namespace Modules.Widgets;

public static class WidgetsModuleSetup
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
        services.AddScoped<IWidgetRepository, WidgetRepository>();
        
        _provider = services.BuildServiceProvider();

        WidgetsCompositionRoot.SetProvider(_provider);
    }
    
    public static void SetupDatabase(Action<MigrationExecutor> action)
    {
        var configuration = _provider.GetRequiredService<IConfiguration>();
        var connection = configuration.GetSystemConnectionString();
        MigrationRunner.Run(connection, action);
    }
}