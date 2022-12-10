using Backend.FunctionalTests.Fixtures;

namespace Backend.FunctionalTests.UseCase.Tenancy;

[Collection(nameof(ServiceCollectionFixture))]
public class GetDashboardTests
{
    private readonly WidgetService.WidgetServiceClient _widgets;
    private readonly TenantStatisticsService.TenantStatisticsServiceClient _statistics;

    public GetDashboardTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _widgets = service.WidgetsClient;
        _statistics = service.TenantStatisticsClient;
    }

    [Fact]
    public async Task CanGetDashboard()
    {
        // arrange
        var id1 = Guid.NewGuid().ToString();
        var id2 = Guid.NewGuid().ToString();
        var id3 = Guid.NewGuid().ToString();
        var tenant1 = MetaDataBuilder.WithTenant();
        var tenant2 = MetaDataBuilder.WithTenant();
        var tenant3 = MetaDataBuilder.WithTenant();

        // act
        var total1 = await _statistics.GetDashboardAsync(new GetDashboardRequest());
        await _widgets.AddWidgetAsync(new AddWidgetRequest {Id = id1}, tenant1);
        await _widgets.AddWidgetAsync(new AddWidgetRequest {Id = id2}, tenant2);
        await _widgets.AddWidgetAsync(new AddWidgetRequest {Id = id3}, tenant3);
        var total2 = await _statistics.GetDashboardAsync(new GetDashboardRequest());

        // assert
        var before = total1.TotalWidgets;
        var after = total2.TotalWidgets;
        after.Should().Be(before + 3);
    }
}