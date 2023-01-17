using Modules.Infrastructure.Tenancy;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Modules.Widgets.IntegrationTests.Fixtures;

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
            .AddWidgetsModule();
        
        services.AddSingleton<IExecutionContextAccessor>(context);
        
        _provider = services.BuildServiceProvider();
        
        WidgetsModuleSetup.Init(context, logging, configuration);
        WidgetsModuleSetup.SetupDatabase(x=>x.ResetDatabase());
    }

    public IServiceProvider Provider => _provider;

    public void Dispose()
    {
        _provider.Dispose();
    }

    public ITestOutputHelper? OutputHelper { get; set; }
}