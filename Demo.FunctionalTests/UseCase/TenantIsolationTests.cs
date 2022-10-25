using Grpc.Core;

namespace Demo.FunctionalTests.UseCase;

[Collection(nameof(ServiceCollectionFixture))]
public class TenantIsolationTests
{
    private readonly TenantService.TenantServiceClient _client;

    public TenantIsolationTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _client = service.TenantClient;
    }

    [Fact]
    public async Task CantGet()
    {
        // arrange
        var tenant1 = MetaDataBuilder.WithTenant("A");
        var tenant2 = MetaDataBuilder.WithTenant("B");
        var id = Guid.NewGuid().ToString();

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id}, tenant1);
        Action act = () => _client.GetCar(new GetCarRequest {Id = id}, tenant2);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*not found*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.NotFound);
    }
    
    [Fact]
    public async Task CantRegister()
    {
        // arrange
        var tenant1 = MetaDataBuilder.WithTenant("A");
        var tenant2 = MetaDataBuilder.WithTenant("B");
        var id = Guid.NewGuid().ToString();
        var registration = Guid.NewGuid().ToString()[..6];

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id}, tenant1);
        Action act = () => _client.RegisterCar(new RegisterCarRequest {Id = id, Registration = registration}, tenant2);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*not found*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.NotFound);
    }
    
    [Fact]
    public async Task CantGetByRegistration()
    {
        // arrange
        var tenant1 = MetaDataBuilder.WithTenant("A");
        var tenant2 = MetaDataBuilder.WithTenant("B");
        var id = Guid.NewGuid().ToString();
        var registration = Guid.NewGuid().ToString()[..6];

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id}, tenant1);
        await _client.RegisterCarAsync(new RegisterCarRequest {Id = id, Registration = registration}, tenant1);
        Action act = () => _client.GetCarByRegistration(new GetCarByRegistrationRequest {Registration = registration}, tenant2);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*not found*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.NotFound);
    }
}