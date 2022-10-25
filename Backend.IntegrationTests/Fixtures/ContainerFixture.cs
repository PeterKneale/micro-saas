using Backend.Infrastructure.Database;

namespace Backend.IntegrationTests.Fixtures;

public class ContainerFixture : IDisposable
{
    private readonly ServiceProvider _provider;

    public ContainerFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        var services = new ServiceCollection();
        services.AddBackend(configuration);
        services.AddSingleton<IConfiguration>(configuration);
        _provider = services.BuildServiceProvider();
        _provider.ExecuteDatabaseMigration(x => x.ResetDatabase());
    }

    public IServiceProvider Provider => _provider;

    public void Dispose()
    {
        _provider.Dispose();
    }
}