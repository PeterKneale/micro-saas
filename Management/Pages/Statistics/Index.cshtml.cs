﻿using Backend.Api;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Statistics;

public class Index : PageModel
{
    private readonly TenantStatisticsService.TenantStatisticsServiceClient _client;
    
    public Index(TenantStatisticsService.TenantStatisticsServiceClient client)
    {
        _client = client;
    }
    
    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var response = await _client.GetDashboardAsync(new GetDashboardRequest(), cancellationToken: cancellationToken);
        Data = response;
    }
    
    public GetDashboardResponse Data { get; private set; }
}