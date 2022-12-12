using Tests.Helpers;

namespace Tests.Tests;

public class ClaimTests : HeadPageTest
{
    [Test]
    public async Task Two_users_can_register_same_identifier_and_first_can_claim()
    {
        var email1 = UniqueHelper.GetUniqueEmail();
        var email2 = UniqueHelper.GetUniqueEmail();
        var identifier = UniqueHelper.GetUniqueIdentifier();

        // Register twice with same identifier
        await Register(email1, identifier);
        await Register(email2, identifier);
        
        // Get Claim link for first user
        var claimLink = await EmailHelper.GetClaimLink(email1);
        
        // Claim for first user
        var claimPage = await Page.GotoClaimPage(claimLink);
        await claimPage.EnterPassword("password");
        await claimPage.ClickClaim();
    }

    [Test]
    public async Task Two_users_can_register_same_identifier_and_second_can_not_claim()
    {
        var email1 = UniqueHelper.GetUniqueEmail();
        var email2 = UniqueHelper.GetUniqueEmail();
        var identifier = UniqueHelper.GetUniqueIdentifier();

        // Register twice with same identifier
        await Register(email1, identifier);
        await Register(email2, identifier);
        
        // claim first
        var claimLink1 = await EmailHelper.GetClaimLink(email1);
        var claimPage1 = await Page.GotoClaimPage(claimLink1);
        await claimPage1.EnterPassword("password");
        await claimPage1.ClickClaim();
        
        // unable to claim second
        var claimLink2 =  await EmailHelper.GetClaimLink(email2);
        var claimPage2 = await Page.GotoClaimPage(claimLink2);
        await claimPage2.EnterPassword("password");
        await claimPage2.ClickClaim();
    }
    
    private async Task Register(string email1, string identifier)
    {
        var registerPage1 = await Page.GotoRegisterPage();
        await registerPage1.EnterName(UniqueHelper.GetUniqueName());
        await registerPage1.EnterEmail(email1);
        await registerPage1.EnterIdentifier(identifier);
        await registerPage1.ClickRegister();
    }
}