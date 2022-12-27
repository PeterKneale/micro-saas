namespace Registration.Pages;

public class Claim : PageModel
{
    private readonly TenantsApi.TenantsApiClient _client;

    public Claim(TenantsApi.TenantsApiClient client)
    {
        _client = client;
    }

    [BindProperty]
    public Model Data { get; set; }

    public IActionResult OnGet()
    {
        if (Identifier == null)
        {
            throw new Exception("Bad link format: Identifier missing");
        }
        if (Token == null)
        {
            throw new Exception("Bad link format: Token missing");
        }
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        await _client.ClaimAsync(new ClaimRequest
        {
            Password = Data.Password,
            Identifier = Identifier,
            Token = Token
        });

        return RedirectToPage(nameof(Claimed));
    }
    
    public record Model
    {
        public string Password { get; init; }
    }
    
    [BindProperty(Name = "identifier", SupportsGet = true)]
    public string? Identifier { get; set; }

    [BindProperty(Name = "token", SupportsGet = true)]
    public string? Token { get; set; }
}