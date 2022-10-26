using Backend.Application.Commands.Tenants;
using Backend.Application.Queries.Tenants;

namespace Backend.IntegrationTests.UseCase;

[Collection(nameof(ContainerCollectionFixture))]
public class TenantIsolationTests
{
    private readonly IServiceProvider _provider;

    public TenantIsolationTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }
    
    [Fact]
    public async Task Different_tenant_cant_list_car()
    {
        // arrange
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();
        var id = Guid.NewGuid();

        // act
        await _provider.ExecuteCommand(new AddCar.Command(id), tenantId1);
        var results = await _provider.ExecuteQuery(new ListCars.Query(), tenantId2);

        // assert
        results.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Different_tenant_cant_get_car()
    {
        // arrange
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();
        var id = Guid.NewGuid();

        // act
        await _provider.ExecuteCommand(new AddCar.Command(id), tenantId1);
        Func<Task> func = async () => { await _provider.ExecuteQuery(new GetCar.Query(id), tenantId2); };

        // assert
        await func.Should().ThrowAsync<Exception>().WithMessage("*not found*");
    }
    
    [Fact]
    public async Task Different_tenant_cant_get_car_by_registration()
    {
        // arrange
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();
        var id = Guid.NewGuid();
        var registration = Guid.NewGuid().ToString()[..6];

        // act
        await _provider.ExecuteCommand(new AddCar.Command(id), tenantId1);
        await _provider.ExecuteCommand(new RegisterCar.Command(id, registration), tenantId1);
        Func<Task> func = async () => { await _provider.ExecuteQuery(new GetCarByRegistration.Query(registration), tenantId2); };

        // assert
        await func.Should().ThrowAsync<Exception>().WithMessage("*not found*");
    }
}