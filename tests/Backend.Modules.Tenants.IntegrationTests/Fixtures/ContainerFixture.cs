using Backend.Modules.Infrastructure.Database;
using DotNetCore.CAP.Internal;

namespace Backend.Modules.Tenants.IntegrationTests.Fixtures;

public class ContainerFixture : IDisposable
{
    private readonly ServiceProvider _provider;

    public ContainerFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .AddEnvironmentVariables()
            .Build();
        
        var services = new ServiceCollection()
            .AddModules(configuration)
            .AddTenants(configuration);
        
        services
            .AddSingleton<IConfiguration>(configuration);
        
        _provider = services.BuildServiceProvider();
        _provider.ExecuteDatabaseMigration(x => x.ResetDatabase());
        
        _provider.GetRequiredService<IBootstrapper>().BootstrapAsync().GetAwaiter().GetResult(); 
    }

    public IServiceProvider Provider => _provider;

    public void Dispose()
    {
        _provider.Dispose();
    }
}