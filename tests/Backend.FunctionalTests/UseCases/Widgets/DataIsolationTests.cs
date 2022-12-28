using Backend.FunctionalTests.Fixtures;
using Grpc.Core;

namespace Backend.FunctionalTests.UseCases.Widgets;

[Collection(nameof(ServiceCollectionFixture))]
public class DataIsolationTests
{
    private readonly WidgetsApi.WidgetsApiClient _client;

    public DataIsolationTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _client = service.WidgetsClient;
    }

    [Fact]
    public async Task CantGet()
    {
        // arrange
        var tenant1 = MetaDataBuilder.WithTenant();
        var tenant2 = MetaDataBuilder.WithTenant();
        var id = Guid.NewGuid().ToString();

        // act
        await _client.AddWidgetAsync(new AddWidgetRequest {Id = id}, tenant1);
        Action act = () => _client.GetWidget(new GetWidgetRequest {Id = id}, tenant2);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*not found*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.NotFound);
    }
    
    [Fact]
    public async Task CantUpdate()
    {
        // arrange
        var tenant1 = MetaDataBuilder.WithTenant();
        var tenant2 = MetaDataBuilder.WithTenant();
        var id = Guid.NewGuid().ToString();
        var registration = Guid.NewGuid().ToString()[..6];

        // act
        await _client.AddWidgetAsync(new AddWidgetRequest {Id = id}, tenant1);
        Action act = () => _client.UpdateWidget(new UpdateWidgetRequest {Id = id, Description = registration}, tenant2);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*not found*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.NotFound);
    }
}