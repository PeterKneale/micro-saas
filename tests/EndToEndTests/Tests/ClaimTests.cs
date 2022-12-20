using EndToEndTests.Helpers;
using EndToEndTests.Pages;

namespace EndToEndTests.Tests;

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
        
        // claim as first user
        await Claim(email1);
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
        await Claim(email1);

        // unable to claim second
        await FailToClaim(email1);
    }
    
    [Test]
    public async Task One_user_can_register_two_identifiers_and_claim_both()
    {
        var email = UniqueHelper.GetUniqueEmail();
        
        var identifier1 = UniqueHelper.GetUniqueIdentifier();
        await Register(email, identifier1);
        await Claim(email);
        
        var identifier2 = UniqueHelper.GetUniqueIdentifier();
        await Register(email, identifier2);
        await Claim(email);
    }
    
    private async Task Claim(string email)
    {
        var link = await EmailHelper.GetFirstClaimLink(email);
        var claim = await Page.GotoClaimPage(link);
        await claim.EnterPassword("password");
        var claimed = await claim.ClickClaim();
        await claimed.AssertClaimedSuccessfully();
    }
    
    private async Task FailToClaim(string email)
    {
        var link = await EmailHelper.GetFirstClaimLink(email);
        var claim = await Page.GotoClaimPage(link);
        await claim.EnterPassword("password");
        await claim.ClickClaimButRemain();
        await claim.AssertClaimFailed();
    }

    private async Task Register(string email1, string identifier)
    {
        var register = await Page.GotoRegisterPage();
        await register.EnterEmail(email1);
        await register.EnterName(UniqueHelper.GetUniqueName());
        await register.EnterIdentifier(identifier);
        await register.ClickRegister();
    }
}