using Backend.Features.Tenancy.Application.Commands;
using Backend.Features.Tenancy.Application.Queries;

namespace Backend.IntegrationTests.UseCase.Tenancy;

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
    
    [Fact]
    public async Task CanAddTenants()
    {
        await _provider.ExecuteCommand(new AddTenant.Command(Guid.NewGuid(), "Tenant A", Guid.NewGuid().ToString()));
        await _provider.ExecuteCommand(new AddTenant.Command(Guid.NewGuid(), "Tenant B", Guid.NewGuid().ToString()));
        await _provider.ExecuteCommand(new AddTenant.Command(Guid.NewGuid(), "Tenant C", Guid.NewGuid().ToString()));
    }
}
