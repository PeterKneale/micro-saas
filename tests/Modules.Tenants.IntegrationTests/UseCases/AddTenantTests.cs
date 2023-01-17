using Modules.Tenants.Application.Commands;
using Modules.Tenants.Application.Queries;
using Modules.Tenants.IntegrationTests.Fixtures;

namespace Modules.Tenants.IntegrationTests.UseCases;

[Collection(nameof(ContainerCollectionFixture))]
public class AddTenantTests
{
    private readonly IServiceProvider _provider;

    public AddTenantTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task CanAddTenant()
    {
        // arrange
        var id = Guid.NewGuid();
        var name = Guid.NewGuid().ToString();
        var identifier = Guid.NewGuid().ToString();

        // act
        await _provider.ExecuteCommand(new AddTenant.Command(id, name, identifier));
        var result = await _provider.ExecuteQuery(new GetTenant.Query(id), id);

        // assert
        result.Id.Should().Be(id);
        result.Name.Should().Be(name);
    }
}