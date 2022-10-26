using Backend.FunctionalTests.Fixtures;
using Grpc.Core;

namespace Backend.FunctionalTests.UseCase.Commands;

[Collection(nameof(ServiceCollectionFixture))]
public class AddCarTests
{
    private readonly TenantService.TenantServiceClient _client;

    public AddCarTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _client = service.TenantClient;
    }

    [Fact]
    public async Task Success()
    {
        // arrange
        var carId = Guid.NewGuid().ToString();
        var tenant = MetaDataBuilder.WithTenant();

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = carId}, tenant);
        var result = await _client.GetCarAsync(new GetCarRequest {Id = carId}, tenant);

        // assert
        result.Id.Should().Be(carId);
        result.Registration.Should().BeEmpty();
    }

    [Fact]
    public async Task AlreadyExists()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var tenant = MetaDataBuilder.WithTenant();

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id}, tenant);
        Action act = () => _client.AddCar(new AddCarRequest {Id = id}, tenant);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*already exists*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.AlreadyExists);
    }
}