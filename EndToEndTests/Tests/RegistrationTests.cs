using EndToEndTests.Helpers;

namespace EndToEndTests.Tests;

public class RegistrationTests : HeadPageTest
{
    [Test]
    public async Task Can_claim_registration()
    {
        var email = UniqueHelper.GetUniqueEmail();
        var name = UniqueHelper.GetUniqueName();
        var identifier = UniqueHelper.GetUniqueIdentifier();

        var register = await Page.GotoRegisterPage();
        await register.EnterEmail(email);
        await register.EnterName(name);
        await register.EnterIdentifier(identifier);
        await register.ClickRegister();

        var link = await EmailHelper.GetFirstClaimLink(email);

        var claim = await Page.GotoClaimPage(link);
        await claim.EnterPassword("password");
        
        var claimed = await claim.ClickClaim();
        await claimed.AssertClaimedSuccessfully();
    }
}