using Backend.Modules.Infrastructure.Tenancy;

namespace Backend.Modules.Registrations.IntegrationTests.Fixtures;

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
            .AddRegistrationsModule();
        
        services.AddSingleton<IExecutionContextAccessor>(context);
        
        _provider = services.BuildServiceProvider();
        RegistrationsModuleSetup.Init(context, logging, configuration);
        RegistrationsModuleSetup.SetupDatabase(x=>x.ResetDatabase());
        RegistrationsModuleSetup.SetupOutbox();
    }

    public IServiceProvider Provider => _provider;

    public void Dispose()
    {
        _provider.Dispose();
    }

    public ITestOutputHelper? OutputHelper { get; set; }
}