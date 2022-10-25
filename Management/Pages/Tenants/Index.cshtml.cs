﻿using Backend.Api;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Management.Pages.Tenants;

public class Index : PageModel
{
    private readonly AdminService.AdminServiceClient _client;
    
    public Index(AdminService.AdminServiceClient client)
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