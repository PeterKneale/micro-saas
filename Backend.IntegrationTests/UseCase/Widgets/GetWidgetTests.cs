using Backend.Features.Widgets.Application.Commands;
using Backend.Features.Widgets.Application.Queries;

namespace Backend.IntegrationTests.UseCase.Widgets;

[Collection(nameof(ContainerCollectionFixture))]
public class GetWidgetTests
{
    private readonly IServiceProvider _provider;

    public GetWidgetTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task WidgetReturned()
    {
        // arrange
        var widgetId = Guid.NewGuid();
        var tenantId =  Guid.NewGuid();

        // act
        await _provider.ExecuteCommand(new AddWidget.Command(widgetId, "x"), tenantId);
        var results = await _provider.ExecuteQuery(new GetWidget.Query(widgetId), tenantId);

        // assert
        results.Id.Should().Be(widgetId);
    }
}