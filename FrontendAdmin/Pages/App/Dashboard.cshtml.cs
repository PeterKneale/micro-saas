using Backend.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontendAdmin.Pages.App;

public class Dashboard : PageModel
{
    private readonly AdminService.AdminServiceClient _client;
    
    public Dashboard(AdminService.AdminServiceClient client)
    {
        _client = client;
    }
    
    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var response = await _client.CountCarsAsync(new CountCarsRequest(), cancellationToken: cancellationToken);
        Total = response.Total;
    }
    
    public int Total { get; private set; }
}