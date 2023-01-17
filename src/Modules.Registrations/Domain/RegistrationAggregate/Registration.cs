using Modules.Registrations.Domain.Common;

namespace Modules.Registrations.Domain.RegistrationAggregate;

public class Registration
{
    private Registration(Email email, TenantName name, TenantIdentifier identifier)
    {
        Id = RegistrationId.CreateInstance();
        Email = email;
        Name = name;
        Identifier = identifier;
        Token = Guid.NewGuid().ToString();
        RegisteredAt = DateTime.UtcNow;
    }

    private Registration()
    {
    }

    public RegistrationId Id { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public TenantName Name { get; private set; } = null!;
    public TenantIdentifier Identifier { get; private set; } = null!;
    public string? Token { get; private set; }
    public DateTime RegisteredAt { get; private set; }
    public DateTime? ClaimedAt { get; private set; }

    public void Claim(string token)
    {
        if (token != Token)
        {
            throw new Exception("Claim failed due to incorrect token");
        }
        Token = null;
        ClaimedAt = DateTime.UtcNow;
    }
    
    // Override the token that will be used to claim this registration
    public void OverrideToken(string token)
    {
        Token = token;
    }

    public static Registration Register(Email email, TenantName name, TenantIdentifier identifier) =>
        new(email, name, identifier);
}