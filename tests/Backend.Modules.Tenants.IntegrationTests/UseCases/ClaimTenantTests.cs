using Backend.Modules.Tenants.Application.Commands;
using Backend.Modules.Tenants.IntegrationTests.Fixtures;

namespace Backend.Modules.Tenants.IntegrationTests.UseCases;

[Collection(nameof(ContainerCollectionFixture))]
public class ClaimTenantTests
{
    private readonly IServiceProvider _provider;

    public ClaimTenantTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task CanClaimTenant()
    {
        // arrange
        var email = $"test{(string?) Guid.NewGuid().ToString()[..8]}@example.com";
        var name = Guid.NewGuid().ToString();
        var identifier = Guid.NewGuid().ToString();
        var password = Guid.NewGuid().ToString();
        var token = Guid.NewGuid().ToString();

        // act
        await _provider.ExecuteCommand(new RegisterTenant.Command(email, name, identifier, token));
        await _provider.ExecuteCommand(new ClaimTenant.Command(identifier, password, token));

        // assert
    }
}