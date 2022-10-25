namespace Demo.IntegrationTests.UseCase;

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
        var tenant1 = Guid.NewGuid().ToString();
        var tenant2 = Guid.NewGuid().ToString();
        var id = Guid.NewGuid();

        // act
        await _provider.ExecuteCommand(new AddCar.Command(id), tenant1);
        var results = await _provider.ExecuteQuery(new ListCars.Query(), tenant2);

        // assert
        results.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Different_tenant_cant_get_car()
    {
        // arrange
        var tenant1 = Guid.NewGuid().ToString();
        var tenant2 = Guid.NewGuid().ToString();
        var id = Guid.NewGuid();

        // act
        await _provider.ExecuteCommand(new AddCar.Command(id), tenant1);
        Func<Task> func = async () => { await _provider.ExecuteQuery(new GetCar.Query(id), tenant2); };

        // assert
        await func.Should().ThrowAsync<Exception>().WithMessage("*not found*");
    }
    
    [Fact]
    public async Task Different_tenant_cant_get_car_by_registration()
    {
        // arrange
        var tenant1 = Guid.NewGuid().ToString();
        var tenant2 = Guid.NewGuid().ToString();
        var id = Guid.NewGuid();
        var registration = Guid.NewGuid().ToString()[..6];

        // act
        await _provider.ExecuteCommand(new AddCar.Command(id), tenant1);
        await _provider.ExecuteCommand(new RegisterCar.Command(id, registration), tenant1);
        Func<Task> func = async () => { await _provider.ExecuteQuery(new GetCarByRegistration.Query(registration), tenant2); };

        // assert
        await func.Should().ThrowAsync<Exception>().WithMessage("*not found*");
    }
}