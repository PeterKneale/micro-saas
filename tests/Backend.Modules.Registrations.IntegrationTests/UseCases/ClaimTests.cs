using Backend.Modules.Registrations.Application.Commands;
using Backend.Modules.Registrations.IntegrationTests.Fixtures;

namespace Backend.Modules.Registrations.IntegrationTests.UseCases;

[Collection(nameof(ContainerCollectionFixture))]
public class ClaimTests
{
    private readonly IServiceProvider _provider;

    public ClaimTests(ContainerFixture container)
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
        await _provider.ExecuteCommand(new Register.Command(email, name, identifier, token));
        await _provider.ExecuteCommand(new Claim.Command(identifier, password, token));

        // assert
    }
}