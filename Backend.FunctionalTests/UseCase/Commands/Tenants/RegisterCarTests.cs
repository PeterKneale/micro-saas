using Backend.FunctionalTests.Fixtures;
using Grpc.Core;

namespace Backend.FunctionalTests.UseCase.Commands;

[Collection(nameof(ServiceCollectionFixture))]
public class RegisterCarTests
{
    private readonly TenantService.TenantServiceClient _client;

    public RegisterCarTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _client = service.TenantClient;
    }

    [Fact]
    public async Task Success()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var registration = Guid.NewGuid().ToString()[..6];
        var tenant = MetaDataBuilder.WithTenant();

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id}, tenant);
        await _client.RegisterCarAsync(new RegisterCarRequest {Id = id, Registration = registration}, tenant);
        var result = await _client.GetCarByRegistrationAsync(new GetCarByRegistrationRequest {Registration = registration}, tenant);

        // assert
        result.Id.Should().Be(id);
        result.Registration.Should().Be(registration);
    }

    [Fact]
    public void NotFound()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var registration = Guid.NewGuid().ToString()[..6];
        var tenant = MetaDataBuilder.WithTenant();

        // act
        Action act = () => _client.RegisterCar(new RegisterCarRequest {Id = id, Registration = registration}, tenant);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*not found*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.NotFound);
    }

    [Fact]
    public async Task Unavailable()
    {
        // arrange
        var id1 = Guid.NewGuid().ToString();
        var id2 = Guid.NewGuid().ToString();
        var registration = Guid.NewGuid().ToString()[..6];
        var tenant = MetaDataBuilder.WithTenant();

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id1}, tenant);
        await _client.AddCarAsync(new AddCarRequest {Id = id2}, tenant);
        await _client.RegisterCarAsync(new RegisterCarRequest {Id = id1, Registration = registration}, tenant);
        Action act = () => _client.RegisterCar(new RegisterCarRequest {Id = id2, Registration = registration}, tenant);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*already exists*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.FailedPrecondition);
    }

    [Fact]
    public async Task AlreadyRegistered()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var registration = Guid.NewGuid().ToString()[..6];
        var tenant = MetaDataBuilder.WithTenant();

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id}, tenant);
        await _client.RegisterCarAsync(new RegisterCarRequest {Id = id, Registration = registration}, tenant);
        Action act = () => _client.RegisterCar(new RegisterCarRequest {Id = id, Registration = registration}, tenant);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*already registered*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.FailedPrecondition);
    }
}