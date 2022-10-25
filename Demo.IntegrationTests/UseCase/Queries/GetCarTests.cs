namespace Demo.IntegrationTests.UseCase.Queries;

[Collection(nameof(ContainerCollectionFixture))]
public class GetCarTests
{
    private readonly IServiceProvider _provider;

    public GetCarTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task CarReturned()
    {
        // arrange
        var carId = Guid.NewGuid();
        var tenant = "A";

        // act
        await _provider.ExecuteCommand(new AddCar.Command(carId), tenant);
        var results = await _provider.ExecuteQuery(new GetCar.Query(carId), tenant);

        // assert
        results.Id.Should().Be(carId);
    }
}