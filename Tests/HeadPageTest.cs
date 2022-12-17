namespace Tests;

public abstract class HeadPageTest : IDisposable
{
   private IPlaywright _playwright;
   private IBrowser _browser;
      
   protected HeadPageTest()
   {
      Setup().GetAwaiter().GetResult();
   }
   
   private async Task Setup()
   {
      _playwright = await Playwright.CreateAsync();
      _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
      {
         Headless = false,
         SlowMo = 100
      });
      Page = await _browser.NewPageAsync();
   }

   protected IPage Page { get; private set; }
   
   public void Dispose()
   {
      _browser?.DisposeAsync().GetAwaiter().GetResult();
      _playwright?.Dispose();
   }
}