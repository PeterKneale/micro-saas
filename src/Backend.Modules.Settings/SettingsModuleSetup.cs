using System.Reflection;
using Backend.Modules.Infrastructure.Database;
using Backend.Modules.Infrastructure.Tenancy;
using Backend.Modules.Settings.Infrastructure;

namespace Backend.Modules.Settings;

public static class SettingsModuleSetup
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

        // infrastructure
        services.AddScoped<ISettingsRepository, SettingsRepository>();
        
        _provider = services.BuildServiceProvider();

        SettingsCompositionRoot.SetProvider(_provider);
    }
    
    public static void SetupDatabase(Action<MigrationExecutor> action)
    {
        using var scope = _provider.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<MigrationExecutor>();
        action(migrator);
    }
}