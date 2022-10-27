namespace Description.Pages;

public class Index : PageModel
{
    private readonly AdminService.AdminServiceClient _client;

    public Index(AdminService.AdminServiceClient client)
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
            Name = Data.Name
        });

        return RedirectToPage("Index");
    }

    public record Model
    {
        public string Name { get; init; }
    }
}