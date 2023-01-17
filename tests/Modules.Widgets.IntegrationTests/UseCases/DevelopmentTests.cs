using System.Linq;
using Modules.Widgets.Application.Commands;

namespace Modules.Widgets.IntegrationTests.UseCases;

[Collection(nameof(ContainerCollectionFixture))]
public class DevelopmentTests
{
    private readonly IServiceProvider _provider;

    public DevelopmentTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task CanAddTenants()
    {
        var tenantA = Guid.NewGuid();
        var tenantB = Guid.NewGuid();
        var tenantC = Guid.NewGuid();

        foreach (var x in Enumerable.Range(1, 100))
        {
            await _provider.ExecuteCommand(new AddWidget.Command(Guid.NewGuid(), $"widget {x}"), tenantA);
            await _provider.ExecuteCommand(new AddWidget.Command(Guid.NewGuid(), $"widget {x}"), tenantB);
            await _provider.ExecuteCommand(new AddWidget.Command(Guid.NewGuid(), $"widget {x}"), tenantC);
        }
    }
}