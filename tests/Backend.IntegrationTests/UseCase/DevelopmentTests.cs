using Backend.Features.Tenancy.Application.Commands;

namespace Backend.IntegrationTests.UseCase;

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

        await _provider.ExecuteCommand(new AddTenant.Command(tenantA, "Tenant A", "A"));
        await _provider.ExecuteCommand(new AddTenant.Command(tenantB, "Tenant B", "B"));
        await _provider.ExecuteCommand(new AddTenant.Command(tenantC, "Tenant C", "C"));

        await _provider.ExecuteCommand(new SetTheme.Command("A"), tenantA);
        await _provider.ExecuteCommand(new SetTheme.Command("B"), tenantB);
        await _provider.ExecuteCommand(new SetTheme.Command("C"), tenantC);
    }
}