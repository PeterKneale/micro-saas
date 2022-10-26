using Backend.Application.Commands.Tenants;
using Backend.Application.Queries.Tenants;

namespace Backend.IntegrationTests.UseCase.Commands.Tenants;

[Collection(nameof(ContainerCollectionFixture))]
public class RegisterCarTests
{
    private readonly IServiceProvider _provider;

    public RegisterCarTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task CanRegisterCar()
    {
        // arrange
        var id = Guid.NewGuid();
        var registration = Guid.NewGuid().ToString()[..6];
        var tenant = Guid.NewGuid();

        // act
        await _provider.ExecuteCommand(new AddCar.Command(id), tenant);
        await _provider.ExecuteCommand(new RegisterCar.Command(id, registration), tenant);
        var result = await _provider.ExecuteQuery(new GetCarByRegistration.Query(registration), tenant);

        // assert
        result.Id.Should().Be(id);
        result.Registration.Should().Be(registration);
    }
}