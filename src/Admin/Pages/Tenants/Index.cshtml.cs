using Backend.Api;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Pages.Tenants;

public class Index : PageModel
{
    private readonly TenantAdminService.TenantAdminServiceClient _client;
    
    public Index(TenantAdminService.TenantAdminServiceClient client)
    {
        _client = client;
    }
    
    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var response = await _client.ListTenantsAsync(new ListTenantsRequest(), cancellationToken: cancellationToken);
        Items = response.Items.ToList();
    }
    
    public IEnumerable<ListTenantsItem> Items { get; private set; }
}