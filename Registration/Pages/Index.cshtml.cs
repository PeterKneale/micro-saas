namespace Registration.Pages;

public class Index : PageModel
{
    private readonly TenantManagementService.TenantManagementServiceClient _client;

    public Index(TenantManagementService.TenantManagementServiceClient client)
    {
        _client = client;
    }

    [BindProperty]
    public Model Data { get; set; }

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