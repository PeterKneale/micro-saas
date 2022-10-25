using Grpc.Core;

namespace Demo.FunctionalTests.UseCase;

[Collection(nameof(ServiceCollectionFixture))]
public class TenantRequestTests
{
    private readonly TenantService.TenantServiceClient _client;

    public TenantRequestTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _client = service.TenantClient;
    }

    [Fact]
    public void MissingTenantMetaDataThrows()
    {
        // arrange
        var id = Guid.NewGuid().ToString();

        // act
        Action act = () => _client.GetCar(new GetCarRequest {Id = id});

        // assert
        act.Should().Throw<RpcException>().WithMessage("*not found*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.PermissionDenied);
    }
}