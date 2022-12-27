using Backend.Modules.Tenants.Application.Commands;
using Backend.Modules.Tenants.Application.Queries;

namespace Backend.IntegrationTests.UseCase.Tenancy;

[Collection(nameof(ContainerCollectionFixture))]
public class ListTenantsTests
{
    private readonly IServiceProvider _provider;

    public ListTenantsTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task TenantsCanBeListed()
    {
        // arrange
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();
        var tenant1 = "A";
        var tenant2 = "B";
        var identifier1 = "a";
        var identifier2 = "b";

        // act
        await _provider.ExecuteCommand(new AddTenant.Command(tenantId1, tenant1, identifier1));
        await _provider.ExecuteCommand(new AddTenant.Command(tenantId2, tenant2, identifier2));
        var results = await _provider.ExecuteQuery(new ListTenants.Query());

        // assert
        results.Should().HaveCountGreaterOrEqualTo(2);
        results.Should().ContainSingle(x => x.Id == tenantId1);
        results.Should().ContainSingle(x => x.Id == tenantId2);
        results.Should().ContainSingle(x => x.Identifier == identifier1);
        results.Should().ContainSingle(x => x.Identifier == identifier2);
    }
}