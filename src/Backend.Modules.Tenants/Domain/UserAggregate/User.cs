using Backend.Modules.Tenants.Domain.Common;

namespace Backend.Modules.Tenants.Domain.UserAggregate;

public class User
{
    private User(TenantId id, UserId userId, Name name, Email email, Password password)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Email = email;
        Password = password;
    }

    private User()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public TenantId Id { get; private set; } = null!;
    public UserId UserId { get; private set; } = null!;
    public Name Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    
    public void SetName(Name name)
    {
        Name = name;
    }

    public static User ProvisionAdministrator(TenantId tenantId, UserId userId, Email email, Password password) =>
        new(tenantId, userId, Name.Administrator, email, password);
}