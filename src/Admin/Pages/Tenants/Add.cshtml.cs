using Backend.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.Pages.Tenants;

public class Add : PageModel
{
    private readonly TenantsApi.TenantsApiClient _client;

    public Add(TenantsApi.TenantsApiClient client)
    {
        _client = client;
    }

    [BindProperty] public Model Data { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        await _client.AddTenantAsync(new AddTenantRequest
        {
            Id = Guid.NewGuid().ToString(),
            Name = Data.Name,
            Identifier = Data.Identifier
        });

        return RedirectToPage("Index");
    }

    public record Model
    {
        public string Name { get; init; }
        public string Identifier { get; init; }
    }
}