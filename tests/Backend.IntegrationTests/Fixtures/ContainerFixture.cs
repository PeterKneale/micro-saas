using Backend.Modules;
using Backend.Modules.Infrastructure.Database;
using Backend.Modules.Settings;
using Backend.Modules.Statistics;
using Backend.Modules.Tenants;
using Backend.Modules.Widgets;

namespace Backend.IntegrationTests.Fixtures;

public class ContainerFixture : IDisposable
{
    private readonly ServiceProvider _provider;

    public ContainerFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        
        var services = new ServiceCollection()
            .AddModules(configuration)
            .AddSettings(configuration)
            .AddStatistics(configuration)
            .AddTenants(configuration)
            .AddWidgets(configuration);
        
        services
            .AddSingleton<IConfiguration>(configuration);
        
        _provider = services.BuildServiceProvider();
        _provider.ExecuteDatabaseMigration(x => x.ResetDatabase());
    }

    public IServiceProvider Provider => _provider;

    public void Dispose()
    {
        _provider.Dispose();
    }
}