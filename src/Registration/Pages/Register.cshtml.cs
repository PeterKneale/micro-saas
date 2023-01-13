namespace Registration.Pages;

public class Register : PageModel
{
    private readonly RegistrationsApi.RegistrationsApiClient _client;

    public Register(RegistrationsApi.RegistrationsApiClient client)
    {
        _client = client;
    }

    [BindProperty]
    public Model Data { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        await _client.RegisterAsync(new RegisterRequest()
        {
            Email = Data.Email,
            Name = Data.Name,
            Identifier = Data.Identifier,
        });

        return RedirectToPage(nameof(Registered));
    }

    public record Model
    {
        public string Email { get; init; }

        public string Name { get; init; }
        
        public string Identifier { get; init; }
    }
}