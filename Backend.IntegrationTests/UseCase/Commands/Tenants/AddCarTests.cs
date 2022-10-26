using Backend.Application.Commands.Tenants;
using Backend.Application.Queries.Tenants;

namespace Backend.IntegrationTests.UseCase.Commands.Tenants;

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
        var tenantId = Guid.NewGuid();

        // act
        await _provider.ExecuteCommand(new AddCar.Command(id), tenantId);
        var result = await _provider.ExecuteQuery(new GetCar.Query(id), tenantId);

        // assert
        result.Id.Should().Be(id);
        result.Registration.Should().BeNull();
    }
}