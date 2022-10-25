using Grpc.Core;

namespace Demo.FunctionalTests.UseCase.Commands;

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

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id}, MetaDataBuilder.WithTenant("A"));
        await _client.RegisterCarAsync(new RegisterCarRequest {Id = id, Registration = registration}, MetaDataBuilder.WithTenant("A"));
        var result = await _client.GetCarByRegistrationAsync(new GetCarByRegistrationRequest {Registration = registration}, MetaDataBuilder.WithTenant("A"));

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

        // act
        Action act = () => _client.RegisterCar(new RegisterCarRequest {Id = id, Registration = registration}, MetaDataBuilder.WithTenant("A"));

        // assert
        act.Should().Throw<RpcException>().WithMessage("*not found*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.NotFound);
    }

    [Fact]
    public async Task AlreadyExists()
    {
        // arrange
        var id1 = Guid.NewGuid().ToString();
        var id2 = Guid.NewGuid().ToString();
        var registration = Guid.NewGuid().ToString()[..6];

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id1}, MetaDataBuilder.WithTenant("A"));
        await _client.AddCarAsync(new AddCarRequest {Id = id2}, MetaDataBuilder.WithTenant("A"));
        await _client.RegisterCarAsync(new RegisterCarRequest {Id = id1, Registration = registration}, MetaDataBuilder.WithTenant("A"));
        Action act = () => _client.RegisterCar(new RegisterCarRequest {Id = id2, Registration = registration}, MetaDataBuilder.WithTenant("A"));

        // assert
        act.Should().Throw<RpcException>().WithMessage("*already exists*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.AlreadyExists);
    }
    
    [Fact]
    public async Task AlreadyRegistered()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var registration = Guid.NewGuid().ToString()[..6];

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id}, MetaDataBuilder.WithTenant("A"));
        await _client.RegisterCarAsync(new RegisterCarRequest {Id = id, Registration = registration}, MetaDataBuilder.WithTenant("A"));
        Action act = () => _client.RegisterCar(new RegisterCarRequest {Id = id, Registration = registration}, MetaDataBuilder.WithTenant("A"));

        // assert
        act.Should().Throw<RpcException>().WithMessage("*already registered*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.FailedPrecondition);
    }
}