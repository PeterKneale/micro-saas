using Modules.Widgets.Application.Commands;
using Modules.Widgets.Application.Queries;

namespace Modules.Widgets.IntegrationTests.UseCases;

[Collection(nameof(ContainerCollectionFixture))]
public class DataIsolationTests
{
    private readonly IServiceProvider _provider;

    public DataIsolationTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task Different_tenant_cant_list_widget()
    {
        // arrange
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();
        var id = Guid.NewGuid();

        // act
        await _provider.ExecuteCommand(new AddWidget.Command(id, "x"), tenantId1);
        var results = await _provider.ExecuteQuery(new ListWidgets.Query(), tenantId2);

        // assert
        results.Should().BeEmpty();
    }

    [Fact]
    public async Task Different_tenant_cant_get_widget()
    {
        // arrange
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();
        var id = Guid.NewGuid();

        // act
        await _provider.ExecuteCommand(new AddWidget.Command(id, "x"), tenantId1);
        Func<Task> func = async () => { await _provider.ExecuteQuery(new GetWidget.Query(id), tenantId2); };

        // assert
        await func.Should().ThrowAsync<Exception>().WithMessage("*not found*");
    }
}