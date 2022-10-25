using Backend.Application.Commands.Admin;
using Backend.Application.Commands.Tenants;
using Backend.Application.Queries.Admin;
using Backend.Application.Queries.Tenants;
using Backend.IntegrationTests.Fixtures;

namespace Backend.IntegrationTests.UseCase.Commands.Admin;

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

        // act
        await _provider.ExecuteCommand(new AddTenant.Command(id, name));
        var result = await _provider.ExecuteQuery(new GetTenant.Query(id), name);

        // assert
        result.Id.Should().Be(id);
        result.Name.Should().Be(name);
    }
}