using Backend.Application.Commands.Tenants;
using Backend.Application.Queries.Tenants;

namespace Backend.IntegrationTests.UseCase.Commands.Tenants;

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
        await _provider.ExecuteCommand(new AddWidget.Command(id), tenantId);
        var result = await _provider.ExecuteQuery(new GetWidget.Query(id), tenantId);

        // assert
        result.Id.Should().Be(id);
        result.Description.Should().BeNull();
    }
}