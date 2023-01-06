using Backend.Modules.Widgets.Application.Commands;
using Backend.Modules.Widgets.Application.Queries;

namespace Backend.Modules.Widgets.IntegrationTests.UseCases;

[Collection(nameof(ContainerCollectionFixture))]
public class AddWidgetTests
{
    private readonly IServiceProvider _module;

    public AddWidgetTests(ContainerFixture container)
    {
        _module = container.Provider;
    }

    [Fact]
    public async Task CanAddWidget()
    {
        // arrange
        var id = Guid.NewGuid();
        var tenantId = Guid.NewGuid();

        // act
        await _module.ExecuteCommand(new AddWidget.Command(id, "x"), tenantId);
        var result = await _module.ExecuteQuery(new GetWidget.Query(id), tenantId);

        // assert
        result.Id.Should().Be(id);
        result.Description.Should().Be("x");
    }
}