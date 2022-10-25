using Backend.FunctionalTests.Fixtures;

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

        // act
        var total1 = await _adminClient.GetDashboardAsync(new GetDashboardRequest());
        await _tenantClient.AddCarAsync(new AddCarRequest {Id = id1}, MetaDataBuilder.WithTenant("A"));
        await _tenantClient.AddCarAsync(new AddCarRequest {Id = id2}, MetaDataBuilder.WithTenant("B"));
        await _tenantClient.AddCarAsync(new AddCarRequest {Id = id3}, MetaDataBuilder.WithTenant("C"));
        var total2 = await _adminClient.GetDashboardAsync(new GetDashboardRequest());

        // assert
        var before = total1.TotalCars;
        var after = total2.TotalCars;
        after.Should().Be(before + 3);
    }
}