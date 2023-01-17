using Modules.Infrastructure.Tenancy;
using Modules.Tenants;
using DotNetCore.CAP.Internal;

namespace Modules.Tenants.IntegrationTests.Fixtures;

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
            .AddTenantsModule();
        
        services.AddSingleton<IExecutionContextAccessor>(context);
        
        _provider = services.BuildServiceProvider();
        TenantsModuleSetup.Init(context, logging, configuration);
        TenantsModuleSetup.SetupDatabase(x=>x.ResetDatabase());
        TenantsModuleSetup.SetupOutbox();
    }

    public IServiceProvider Provider => _provider;

    public void Dispose()
    {
        _provider.Dispose();
    }

    public ITestOutputHelper? OutputHelper { get; set; }
}