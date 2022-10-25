namespace Demo.IntegrationTests.UseCase.Commands;

[Collection(nameof(ContainerCollectionFixture))]
public class AddCarTests
{
    private readonly IServiceProvider _provider;

    public AddCarTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task CanAddCar()
    {
        // arrange
        var id = Guid.NewGuid();
        var tenant = "A";

        // act
        await _provider.ExecuteCommand(new AddCar.Command(id), tenant);
        var result = await _provider.ExecuteQuery(new GetCar.Query(id), tenant);

        // assert
        result.Id.Should().Be(id);
        result.Registration.Should().BeNull();
    }
}