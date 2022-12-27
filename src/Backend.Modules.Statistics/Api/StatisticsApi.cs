using Backend.Api;
using Backend.Modules.Statistics.Application.Queries;

namespace Backend.Modules.Statistics.Api;

public class StatisticsApi : Backend.Api.StatisticsApi.StatisticsApiBase
{
    private readonly IMediator _mediator;

    public StatisticsApi(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async Task<GetDashboardResponse> GetDashboard(GetDashboardRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetStatistics.Query());
        return new GetDashboardResponse
        {
            TotalTenants = result.TotalTenants,
            TotalWidgets = result.TotalWidgets
        };
    }
}