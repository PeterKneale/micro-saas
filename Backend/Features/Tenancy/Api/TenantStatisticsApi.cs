using Backend.Features.Tenancy.Application.Commands;
using Backend.Features.Tenancy.Application.Queries;

namespace Backend.Features.Tenancy.Api;

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