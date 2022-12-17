namespace Tests.Pages;

public class RegisterPage : PageModel
{
    public RegisterPage(IPage page) : base(page)
    {
    }

    public async Task EnterName(string name) =>
        await Page
            .GetByTestId("Name")
            .FillAsync(name);

    public async Task EnterEmail(string email) =>
        await Page
            .GetByTestId("Email")
            .FillAsync(email);

    public async Task EnterIdentifier(string identifier) =>
        await Page
            .GetByTestId("Identifier")
            .FillAsync(identifier);

    public async Task ClickRegister() =>
        await Page
            .GetByTestId("Register")
            .ClickAsync();
}