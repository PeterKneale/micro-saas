﻿using Backend.FunctionalTests.Fixtures;

namespace Backend.FunctionalTests.UseCase.Queries.Admin;

[Collection(nameof(ServiceCollectionFixture))]
public class GetDashboardTests
{
    private readonly TenantService.TenantServiceClient _tenantClient;
    private readonly AdminService.AdminServiceClient _adminClient;

    public GetDashboardTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _tenantClient = service.TenantClient;
        _adminClient = service.AdminClient;
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
        var total1 = await _adminClient.GetDashboardAsync(new GetDashboardRequest());
        await _tenantClient.AddWidgetAsync(new AddWidgetRequest {Id = id1}, tenant1);
        await _tenantClient.AddWidgetAsync(new AddWidgetRequest {Id = id2}, tenant2);
        await _tenantClient.AddWidgetAsync(new AddWidgetRequest {Id = id3}, tenant3);
        var total2 = await _adminClient.GetDashboardAsync(new GetDashboardRequest());

        // assert
        var before = total1.TotalWidgets;
        var after = total2.TotalWidgets;
        after.Should().Be(before + 3);
    }
}