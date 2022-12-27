using Backend.Api;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Pages.Statistics;

public class Index : PageModel
{
    private readonly StatisticsApi.StatisticsApiClient _client;
    
    public Index(StatisticsApi.StatisticsApiClient client)
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