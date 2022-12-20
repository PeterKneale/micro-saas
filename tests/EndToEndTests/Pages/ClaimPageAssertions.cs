using FluentAssertions;

namespace EndToEndTests.Pages;

public static class ClaimPageAssertions
{
    public static async Task AssertClaimFailed(this ClaimPage page)
    { 
        var title = await page.GetTitle;
        title.Should().Contain("Error");
    }
}