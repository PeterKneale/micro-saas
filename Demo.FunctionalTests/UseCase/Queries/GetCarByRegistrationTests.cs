using Grpc.Core;

namespace Demo.FunctionalTests.UseCase.Queries;

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

        // act
        Action act = () => _client.GetCarByRegistration(new GetCarByRegistrationRequest {Registration = registration}, MetaDataBuilder.WithTenant("A"));

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

        // act
        Action act = () => _client.GetCarByRegistration(new GetCarByRegistrationRequest {Registration = registration}, MetaDataBuilder.WithTenant("A"));

        // assert
        act.Should().Throw<RpcException>().WithMessage("*'Registration' must not be empty*")
            .And
            .Status.StatusCode.Should().Be(StatusCode.InvalidArgument);
    }
}