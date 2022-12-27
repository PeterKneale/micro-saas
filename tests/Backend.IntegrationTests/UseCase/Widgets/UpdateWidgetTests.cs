using Backend.Modules.Widgets.Application.Commands;
using Backend.Modules.Widgets.Application.Queries;

namespace Backend.IntegrationTests.UseCase.Widgets;

[Collection(nameof(ContainerCollectionFixture))]
public class UpdateWidgetTests
{
    private readonly IServiceProvider _provider;

    public UpdateWidgetTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task CanUpdateWidget()
    {
        // arrange
        var id = Guid.NewGuid();
        var registration = Guid.NewGuid().ToString()[..6];
        var tenant = Guid.NewGuid();

        // act
        await _provider.ExecuteCommand(new AddWidget.Command(id, "x"), tenant);
        await _provider.ExecuteCommand(new UpdateWidget.Command(id, registration), tenant);
        var result = await _provider.ExecuteQuery(new GetWidget.Query(id), tenant);

        // assert
        result.Id.Should().Be(id);
        result.Description.Should().Be(registration);
    }
}