using Backend.Modules.Statistics;
using Backend.Modules.Statistics.Application.Queries;
using Grpc.Core;

namespace Backend.Api;

public class StatisticsApiService : StatisticsApi.StatisticsApiBase
{
    private readonly IStatisticsModule _module;

    public StatisticsApiService(IStatisticsModule module)
    {
        _module = module;
    }
    
    public override async Task<GetDashboardResponse> GetDashboard(GetDashboardRequest request, ServerCallContext context)
    {
        var result = await _module.ExecuteQueryAsync(new GetStatistics.Query());
        return new GetDashboardResponse
        {
            TotalTenants = result.TotalTenants,
            TotalWidgets = result.TotalWidgets
        };
    }
}