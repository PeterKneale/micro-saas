using Backend.FunctionalTests.Fixtures;
using Grpc.Core;

namespace Backend.FunctionalTests.UseCase.Commands.Tenants;

[Collection(nameof(ServiceCollectionFixture))]
public class UpdateWidgetTests
{
    private readonly TenantService.TenantServiceClient _client;

    public UpdateWidgetTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _client = service.TenantClient;
    }

    [Fact]
    public async Task Success()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var registration = Guid.NewGuid().ToString()[..6];
        var tenant = MetaDataBuilder.WithTenant();

        // act
        await _client.AddWidgetAsync(new AddWidgetRequest {Id = id}, tenant);
        await _client.UpdateWidgetAsync(new UpdateWidgetRequest {Id = id, Description = registration}, tenant);
        var result = await _client.GetWidgetAsync(new GetWidgetRequest {Id = id}, tenant);

        // assert
        result.Id.Should().Be(id);
        result.Description.Should().Be(registration);
    }

    [Fact]
    public void NotFound()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var registration = Guid.NewGuid().ToString()[..6];
        var tenant = MetaDataBuilder.WithTenant();

        // act
        Action act = () => _client.UpdateWidget(new UpdateWidgetRequest {Id = id, Description = registration}, tenant);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*not found*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.NotFound);
    }
}