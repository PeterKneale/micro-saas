using Backend.FunctionalTests.Fixtures;
using Grpc.Core;

namespace Backend.FunctionalTests.UseCase;

[Collection(nameof(ServiceCollectionFixture))]
public class TenantRequestTests
{
    private readonly WidgetsApi.WidgetsApiClient _client;

    public TenantRequestTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _client = service.WidgetsClient;
    }

    [Fact]
    public void MissingTenantMetaDataThrows()
    {
        // arrange
        var id = Guid.NewGuid().ToString();

        // act
        Action act = () => _client.GetWidget(new GetWidgetRequest {Id = id});

        // assert
        act.Should().Throw<RpcException>().WithMessage("*not found*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.PermissionDenied);
    }
}