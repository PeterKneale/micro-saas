namespace Demo.FunctionalTests.UseCase.Queries;

[Collection(nameof(ServiceCollectionFixture))]
public class CountCarsTests
{
    private readonly TenantService.TenantServiceClient _tenantClient;
    private readonly AdminService.AdminServiceClient _adminClient;

    public CountCarsTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _tenantClient = service.TenantClient;
        _adminClient = service.AdminClient;
    }

    [Fact]
    public async Task Success()
    {
        // arrange
        var id1 = Guid.NewGuid().ToString();
        var id2 = Guid.NewGuid().ToString();
        var id3 = Guid.NewGuid().ToString();

        // act
        var total1 = await _adminClient.CountCarsAsync(new CountCarsRequest());
        await _tenantClient.AddCarAsync(new AddCarRequest {Id = id1}, MetaDataBuilder.WithTenant("A"));
        await _tenantClient.AddCarAsync(new AddCarRequest {Id = id2}, MetaDataBuilder.WithTenant("B"));
        await _tenantClient.AddCarAsync(new AddCarRequest {Id = id3}, MetaDataBuilder.WithTenant("C"));
        var total2 = await _adminClient.CountCarsAsync(new CountCarsRequest());

        // assert
        var before = total1.Total;
        var after = total2.Total;
        after.Should().Be(before + 3);
    }
}