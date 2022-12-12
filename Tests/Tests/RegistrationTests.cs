using Tests.Helpers;

namespace Tests.Tests;

public class RegistrationTests : HeadPageTest
{
    [Test]
    public async Task Can_claim_registration()
    {
        var email = UniqueHelper.GetUniqueEmail();
        var name = UniqueHelper.GetUniqueName();
        var identifier = UniqueHelper.GetUniqueIdentifier();

        var registerPage = await Page.GotoRegisterPage();
        await registerPage.EnterEmail(email);
        await registerPage.EnterName(name);
        await registerPage.EnterIdentifier(identifier);
        await registerPage.ClickRegister();

        var claimLink = await EmailHelper.GetClaimLink(email);

        var claimPage = await Page.GotoClaimPage(claimLink);
        await claimPage.EnterPassword("password");
        await claimPage.ClickClaim();
    }
}