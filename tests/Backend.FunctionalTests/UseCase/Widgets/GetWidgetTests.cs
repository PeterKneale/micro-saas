using Backend.FunctionalTests.Fixtures;
using Grpc.Core;

namespace Backend.FunctionalTests.UseCase.Widgets;

[Collection(nameof(ServiceCollectionFixture))]
public class GetWidgetTests
{
    private readonly WidgetService.WidgetServiceClient _client;

    public GetWidgetTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _client = service.WidgetsClient;
    }

    [Fact]
    public async Task Success()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var tenant = MetaDataBuilder.WithTenant();

        // act
        await _client.AddWidgetAsync(new AddWidgetRequest {Id = id}, tenant);
        var result = await _client.GetWidgetAsync(new GetWidgetRequest {Id = id}, tenant);

        // assert
        result.Id.Should().Be(id);
        result.Description.Should().BeEmpty();
    }

    [Fact]
    public void NotFound()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var tenant = MetaDataBuilder.WithTenant();

        // act
        Action act = () => _client.GetWidget(new GetWidgetRequest {Id = id}, tenant);

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
        Action act = () => _client.GetWidget(new GetWidgetRequest {Id = id}, tenant);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*'Id' must be a valid GUID*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.InvalidArgument);
    }
}