using FluentAssertions;

namespace EndToEndTests.Pages;

public static class ClaimedPageAssertions
{
    public static async Task AssertClaimedSuccessfully(this ClaimedPage page)
    { 
        var title = await page.GetTitle;
        title.Should().Contain("Claimed");
    }
}