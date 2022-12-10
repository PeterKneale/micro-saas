using Backend.Features.Tenancy.Domain.TenantAggregate;

namespace Backend.Features.Tenancy.Domain.UserAggregate;

public class User
{
    private User(TenantId id, UserId userId, Name userName, Email email, Password password, string? token)
    {
        Id = id;
        UserId = userId;
        UserName = userName;
        Email = email;
        Password = password;
        Token = token;
    }

    private User()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public TenantId Id { get; private set; } = null!;
    public UserId UserId { get; private set; } = null!;
    public Name UserName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;

    public bool EmailVerified { get; set; }

    public Password Password { get; private set; } = null!;
    public string? Token { get; private set; }

    public void SetName(Name name)
    {
        UserName = name;
    }

    public void VerifyEmail(string token)
    {
        if (token != Token)
        {
            throw new Exception("Token is invalid");
        }
        EmailVerified = true;
        Token = null;
    }

    public static User ProvisionAdministrator(TenantId tenantId, UserId userId, Email email, string token) =>
        new(tenantId, userId, Name.Administrator, email, Password.Empty(), token);
}