using Backend.FunctionalTests.Fixtures;
using Grpc.Core;

namespace Backend.FunctionalTests.UseCase.Commands.Widgets;

[Collection(nameof(ServiceCollectionFixture))]
public class AddWidgetTests
{
    private readonly WidgetService.WidgetServiceClient _client;

    public AddWidgetTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _client = service.WidgetsClient;
    }

    [Fact]
    public async Task Success()
    {
        // arrange
        var widgetId = Guid.NewGuid().ToString();
        var tenant = MetaDataBuilder.WithTenant();

        // act
        await _client.AddWidgetAsync(new AddWidgetRequest {Id = widgetId}, tenant);
        var result = await _client.GetWidgetAsync(new GetWidgetRequest {Id = widgetId}, tenant);

        // assert
        result.Id.Should().Be(widgetId);
        result.Description.Should().BeEmpty();
    }

    [Fact]
    public async Task AlreadyExists()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var tenant = MetaDataBuilder.WithTenant();

        // act
        await _client.AddWidgetAsync(new AddWidgetRequest {Id = id}, tenant);
        Action act = () => _client.AddWidget(new AddWidgetRequest {Id = id}, tenant);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*already exists*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.AlreadyExists);
    }
}