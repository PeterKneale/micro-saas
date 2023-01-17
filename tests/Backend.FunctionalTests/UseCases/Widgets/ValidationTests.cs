using Backend.FunctionalTests.Fixtures;
using Grpc.Core;

namespace Backend.FunctionalTests.UseCases.Widgets;

[Collection(nameof(ServiceCollectionFixture))]
public class ValidationTests
{
    private readonly WidgetsApi.WidgetsApiClient _client;

    public ValidationTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _client = service.WidgetsClient;
    }

    [Fact]
    public void InvalidRequestReturnsInvalidArgument()
    {
        // arrange
        var id = "X";
        var tenant = MetaDataBuilder.WithTenant();

        // act
        Action act = () => _client.GetWidget(new GetWidgetRequest {Id = id}, tenant);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*'Id' must be a valid GUID*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.InvalidArgument);
    }
    
    [Fact]
    public void MissingTenantMetaDataThrows()
    {
        // arrange
        var id = Guid.NewGuid().ToString();

        // act
        Action act = () => _client.GetWidget(new GetWidgetRequest {Id = id});

        // assert
        act.Should().Throw<RpcException>().WithMessage("*Tenant Header is not available*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.PermissionDenied);
    }
}