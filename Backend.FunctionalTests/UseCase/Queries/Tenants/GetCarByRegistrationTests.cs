using Backend.FunctionalTests.Fixtures;
using Grpc.Core;

namespace Backend.FunctionalTests.UseCase.Queries;

[Collection(nameof(ServiceCollectionFixture))]
public class GetCarByRegistrationTests
{
    private readonly TenantService.TenantServiceClient _client;

    public GetCarByRegistrationTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _client = service.TenantClient;
    }

    [Fact]
    public void NotFound()
    {
        // arrange
        var registration = Guid.NewGuid().ToString()[..6];
        var tenant = MetaDataBuilder.WithTenant();

        // act
        Action act = () => _client.GetCarByRegistration(new GetCarByRegistrationRequest {Registration = registration}, tenant);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*not found*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.NotFound);
    }


    [Fact]
    public void BadRequest()
    {
        // arrange
        var registration = string.Empty;
        var tenant = MetaDataBuilder.WithTenant();

        // act
        Action act = () => _client.GetCarByRegistration(new GetCarByRegistrationRequest {Registration = registration}, tenant);

        // assert
        act.Should().Throw<RpcException>().WithMessage("*'Registration' must not be empty*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.InvalidArgument);
    }
}