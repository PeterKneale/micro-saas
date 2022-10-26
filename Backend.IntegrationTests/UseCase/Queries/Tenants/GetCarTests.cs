using Backend.Application.Commands.Tenants;
using Backend.Application.Queries.Tenants;

namespace Backend.IntegrationTests.UseCase.Queries.Tenants;

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
        var tenantId =  Guid.NewGuid();

        // act
        await _provider.ExecuteCommand(new AddCar.Command(carId), tenantId);
        var results = await _provider.ExecuteQuery(new GetCar.Query(carId), tenantId);

        // assert
        results.Id.Should().Be(carId);
    }
}