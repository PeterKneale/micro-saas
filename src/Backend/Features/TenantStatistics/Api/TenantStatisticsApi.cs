using Backend.Features.TenantStatistics.Application.Queries;

namespace Backend.Features.TenantStatistics.Api;

public class TenantStatisticsApi : TenantStatisticsService.TenantStatisticsServiceBase
{
    private readonly IMediator _mediator;

    public TenantStatisticsApi(IMediator mediator)
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