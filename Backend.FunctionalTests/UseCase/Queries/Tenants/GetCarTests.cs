using Backend.FunctionalTests.Fixtures;
using Grpc.Core;

namespace Backend.FunctionalTests.UseCase.Queries;

[Collection(nameof(ServiceCollectionFixture))]
public class GetCarTests
{
    private readonly TenantService.TenantServiceClient _client;

    public GetCarTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _client = service.TenantClient;
    }

    [Fact]
    public async Task Success()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var tenant = MetaDataBuilder.WithTenant();

        // act
        await _client.AddCarAsync(new AddCarRequest {Id = id}, tenant);
        var result = await _client.GetCarAsync(new GetCarRequest {Id = id}, tenant);

        // assert
        result.Id.Should().Be(id);
        result.Registration.Should().BeEmpty();
    }

    [Fact]
    public void NotFound()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var tenant = MetaDataBuilder.WithTenant();

        // act
        Action act = () => _client.GetCar(new GetCarRequest {Id = id}, tenant);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*not found*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.NotFound);
    }


    [Fact]
    public void BadRequest()
    {
        // arrange
        var id = "X";
        var tenant = MetaDataBuilder.WithTenant();

        // act
        Action act = () => _client.GetCar(new GetCarRequest {Id = id}, tenant);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*'Id' must be a valid GUID*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.InvalidArgument);
    }
}