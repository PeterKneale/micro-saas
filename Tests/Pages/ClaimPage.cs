namespace Tests.Pages;

public class ClaimPage
{
    private readonly IPage _page;

    public ClaimPage(IPage page)
    {
        _page = page;
    }
    
    public async Task EnterPassword(string password) =>
        await _page
            .GetByTestId("Password")
            .FillAsync(password);
    
    public async Task ClickClaim() =>
        await _page.GetByTestId("Claim").ClickAsync();
}