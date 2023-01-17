using Backend.Modules.Infrastructure.Database;
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
        WidgetsClient = new WidgetsApi.WidgetsApiClient(_channel);
        TenantAdminClient = new TenantsApi.TenantsApiClient(_channel);
        TenantStatisticsClient = new StatisticsApi.StatisticsApiClient(_channel);
        TenantSettingsClient = new SettingsApi.SettingsApiClient(_channel);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureLogging(x => x.AddXUnit(this));

    public ITestOutputHelper? OutputHelper { get; set; }
    public TenantsApi.TenantsApiClient TenantAdminClient { get; }
    public SettingsApi.SettingsApiClient TenantSettingsClient { get; set; }
    public StatisticsApi.StatisticsApiClient TenantStatisticsClient { get; }
    public WidgetsApi.WidgetsApiClient WidgetsClient { get; }

    protected override void Dispose(bool disposing)
    {
        _channel.Dispose();
        base.Dispose(disposing);
    }
}