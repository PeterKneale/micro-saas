namespace Tests.Pages;

public class RegisterPage
{
    private readonly IPage _page;

    public RegisterPage(IPage page)
    {
        _page = page;
    }

    public async Task EnterName(string name) =>
        await _page
            .GetByTestId("Name")
            .FillAsync(name);
    
    public async Task EnterEmail(string email) =>
        await _page
            .GetByTestId("Email")
            .FillAsync(email);

    public async Task EnterIdentifier(string identifier) =>
        await _page
            .GetByTestId("Identifier")
            .FillAsync(identifier);

    public async Task ClickRegister() =>
        await _page
            .GetByTestId("Register")
            .ClickAsync();
}