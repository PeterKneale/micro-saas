using EndToEndTests.Helpers;
using EndToEndTests.Pages;

namespace EndToEndTests;

public static class Extensions
{
    public static async Task<RegisterPage> GotoRegisterPage(this IPage page)
    {
        await page.GotoAsync(UriHelper.Registration.Register.ToString());
        return new RegisterPage(page);
    }
    
    public static async Task<ClaimPage> GotoClaimPage(this IPage page, string claimLink)
    {
        await page.GotoAsync(claimLink);
        return new ClaimPage(page);
    }
}