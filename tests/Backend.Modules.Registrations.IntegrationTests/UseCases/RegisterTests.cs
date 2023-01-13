using Backend.Modules.Registrations.Application.Commands;
using Backend.Modules.Registrations.IntegrationTests.Fixtures;

namespace Backend.Modules.Registrations.IntegrationTests.UseCases;

[Collection(nameof(ContainerCollectionFixture))]
public class RegisterTests
{
    private readonly IServiceProvider _provider;

    public RegisterTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task CanRegisterTenant()
    {
        // arrange
        var email = $"test{Guid.NewGuid()}@example.com";
        var name = Guid.NewGuid().ToString();
        var identifier = Guid.NewGuid().ToString();

        // act
        await _provider.ExecuteCommand(new Register.Command(email, name, identifier));

        // assert
    }

    [Fact]
    public async Task User_can_register_multiple_tenants()
    {
        // arrange
        var email = $"test{Guid.NewGuid()}@example.com";

        var name1 = Guid.NewGuid().ToString();
        var identifier1 = Guid.NewGuid().ToString();

        var name2 = Guid.NewGuid().ToString();
        var identifier2 = Guid.NewGuid().ToString();

        // act
        await _provider.ExecuteCommand(new Register.Command(email, name1, identifier1));
        await _provider.ExecuteCommand(new Register.Command(email, name2, identifier2));

        // assert
    }

    [Fact]
    public async Task Users_can_register_same_tenant()
    {
        // arrange
        var email1 = $"test{Guid.NewGuid()}@example.com";
        var email2 = $"test{Guid.NewGuid()}@example.com";

        var name = Guid.NewGuid().ToString();
        var identifier = Guid.NewGuid().ToString();

        // act
        await _provider.ExecuteCommand(new Register.Command(email1, name, identifier));
        await _provider.ExecuteCommand(new Register.Command(email2, name, identifier));

        // assert
    }
}