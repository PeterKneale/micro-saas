namespace Demo.IntegrationTests.UseCase.Commands;

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
        var tenant = "A";

        // act
        await _provider.ExecuteCommand(new AddCar.Command(id), tenant);
        await _provider.ExecuteCommand(new RegisterCar.Command(id, registration), tenant);
        var result = await _provider.ExecuteQuery(new GetCarByRegistration.Query(registration), tenant);

        // assert
        result.Id.Should().Be(id);
        result.Registration.Should().Be(registration);
    }
}