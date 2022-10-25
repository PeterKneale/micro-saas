using Grpc.Core;

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
        var id = Guid.NewGuid().ToString();

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id}, MetaDataBuilder.WithTenant("A"));
        var result = await _client.GetCarAsync(new GetCarRequest {Id = id}, MetaDataBuilder.WithTenant("A"));

        // assert
        result.Id.Should().Be(id);
        result.Registration.Should().BeEmpty();
    }

    [Fact]
    public async Task AlreadyExists()
    {
        // arrange
        var id = Guid.NewGuid().ToString();

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id}, MetaDataBuilder.WithTenant("A"));
        Action act = () => _client.AddCar(new AddCarRequest {Id = id}, MetaDataBuilder.WithTenant("A"));

        // assert
        act.Should().Throw<RpcException>().WithMessage("*already exists*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.AlreadyExists);
    }
}