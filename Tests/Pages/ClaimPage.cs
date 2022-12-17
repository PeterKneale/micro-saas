namespace Tests.Pages;

public class ClaimPage : PageModel
{
    public ClaimPage(IPage page) : base(page)
    {
    }

    public async Task EnterPassword(string password) =>
        await Page
            .GetByTestId("Password")
            .FillAsync(password);

    public async Task<ClaimedPage> ClickClaim()
    {
        await Page.GetByTestId("Claim").ClickAsync();
        return new ClaimedPage(Page);
    }

    public async Task ClickClaimButRemain()
    {
        await Page.GetByTestId("Claim").ClickAsync();
    }
}