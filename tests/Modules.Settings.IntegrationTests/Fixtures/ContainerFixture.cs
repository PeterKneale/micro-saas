using Modules.Infrastructure.Database;
using Modules.Infrastructure.Tenancy;
using Microsoft.Extensions.Logging;

namespace Modules.Settings.IntegrationTests.Fixtures;

public class ContainerFixture : IDisposable
{
    private readonly ServiceProvider _provider;

    public ContainerFixture()
    {
        var context = new FakeExecutionContextAccessor();
        
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .AddEnvironmentVariables()
            .Build();

        var logging = LoggerFactory.Create(c =>
        {
            c.AddConsole();
        });

        var services = new ServiceCollection()
            .AddSettingsModule();
        
        services.AddSingleton<IExecutionContextAccessor>(context);
        
        _provider = services.BuildServiceProvider();
        
        SettingsModuleSetup.Init(context, logging, configuration);
        SettingsModuleSetup.SetupDatabase(x=>x.ResetDatabase());
    }

    public IServiceProvider Provider => _provider;

    public void Dispose()
    {
        _provider.Dispose();
    }
}