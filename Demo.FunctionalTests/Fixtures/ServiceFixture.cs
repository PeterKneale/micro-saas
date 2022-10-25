using Demo.Infrastructure.Database;
using Grpc.Net.Client;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace Demo.FunctionalTests.Fixtures;

public class ServiceFixture : WebApplicationFactory<ApiAssembly>, ITestOutputHelperAccessor
{
    private readonly GrpcChannel _channel;

    public ServiceFixture()
    {
        var httpClient = CreateDefaultClient();
        _channel = GrpcChannel.ForAddress(httpClient.BaseAddress!, new GrpcChannelOptions
        {
            HttpClient = httpClient
        });
        TenantClient = new TenantService.TenantServiceClient(_channel);
        AdminClient = new AdminService.AdminServiceClient(_channel);
        Services.ExecuteDatabaseMigration(x => x.ResetDatabase());
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureLogging(x => x.AddXUnit(this));

    public ITestOutputHelper? OutputHelper { get; set; }

    public TenantService.TenantServiceClient TenantClient { get; }
    public AdminService.AdminServiceClient AdminClient { get; }

    protected override void Dispose(bool disposing)
    {
        _channel.Dispose();
        base.Dispose(disposing);
    }
}