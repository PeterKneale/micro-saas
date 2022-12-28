using Backend.Modules.Widgets.Application.Commands;
using Backend.Modules.Widgets.Application.Queries;

namespace Backend.Modules.Widgets.IntegrationTests.UseCases;

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