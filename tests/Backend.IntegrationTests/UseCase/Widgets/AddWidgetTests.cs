using Backend.Modules.Widgets.Application.Commands;
using Backend.Modules.Widgets.Application.Queries;

namespace Backend.IntegrationTests.UseCase.Widgets;

[Collection(nameof(ContainerCollectionFixture))]
public class AddWidgetTests
{
    private readonly IServiceProvider _provider;

    public AddWidgetTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task CanAddWidget()
    {
        // arrange
        var id = Guid.NewGuid();
        var tenantId = Guid.NewGuid();

        // act
        await _provider.ExecuteCommand(new AddWidget.Command(id, "x"), tenantId);
        var result = await _provider.ExecuteQuery(new GetWidget.Query(id), tenantId);

        // assert
        result.Id.Should().Be(id);
        result.Description.Should().Be("x");
    }
}