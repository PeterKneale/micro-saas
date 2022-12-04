using Microsoft.Playwright;

namespace Tests;

public class Tests
{
    [Test]
    public async Task RegisterAndLogin()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            SlowMo = 1000
        });
        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();
        var unique = Guid.NewGuid().ToString()[..6];
        
        await page.GotoAsync(Uris.Registration.Home.ToString());
        await page.GetByTestId("Name").FillAsync("Name" + unique);
        await page.GetByTestId("Identifier").FillAsync("Identifier" + unique);
        await page.GetByTestId("Register").ClickAsync();

        var url = Uris.FrontEnd.Login.ToString();
        await page.GotoAsync(url);
        await page.GetByTestId("Email").FillAsync("admin@example.org");
        await page.GetByTestId("Password").FillAsync("password");
        await page.GetByTestId("Login").ClickAsync();
        
        await page.GetByTestId("Logout").ClickAsync();
    }
}