using Modules.Tenants.Application.Commands;
using Modules.Tenants.IntegrationTests.Fixtures;

namespace Modules.Tenants.IntegrationTests.UseCases;

[Collection(nameof(ContainerCollectionFixture))]
public class DevelopmentTests
{
    private readonly IServiceProvider _provider;

    public DevelopmentTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task SeedTenants()
    {
        var tenantA = Guid.NewGuid();
        var tenantB = Guid.NewGuid();
        var tenantC = Guid.NewGuid();

        await _provider.ExecuteCommand(new AddTenant.Command(tenantA, "Tenant A", "A"));
        await _provider.ExecuteCommand(new AddTenant.Command(tenantB, "Tenant B", "B"));
        await _provider.ExecuteCommand(new AddTenant.Command(tenantC, "Tenant C", "C"));
    }

    [Fact]
    public void EmptyDatabase()
    {
    }
}