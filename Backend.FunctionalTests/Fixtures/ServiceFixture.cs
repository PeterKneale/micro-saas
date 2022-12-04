using Backend.Core.Infrastructure.Database;
using Grpc.Net.Client;
using MartinCostello.Logging.XUnit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace Backend.FunctionalTests.Fixtures;

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
        WidgetsClient = new WidgetService.WidgetServiceClient(_channel);
        TenantAdminClient = new TenantAdminService.TenantAdminServiceClient(_channel);
        TenantStatisticsClient = new TenantStatisticsService.TenantStatisticsServiceClient(_channel);
        Services.ExecuteDatabaseMigration(x => x.ResetDatabase());
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureLogging(x => x.AddXUnit(this));

    public ITestOutputHelper? OutputHelper { get; set; }

    public WidgetService.WidgetServiceClient WidgetsClient { get; }
    public TenantAdminService.TenantAdminServiceClient TenantAdminClient { get; }
    public TenantStatisticsService.TenantStatisticsServiceClient TenantStatisticsClient { get; }

    protected override void Dispose(bool disposing)
    {
        _channel.Dispose();
        base.Dispose(disposing);
    }
}