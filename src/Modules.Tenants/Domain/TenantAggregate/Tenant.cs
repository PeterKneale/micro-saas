namespace Modules.Tenants.Domain.TenantAggregate;

public class Tenant
{
    private Tenant(TenantId id, TenantName name, TenantIdentifier identifier)
    {
        Id = id;
        Name = name;
        TenantIdentifier = identifier;
    }

    private Tenant()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public TenantId Id { get; private set; } = null!;
    public TenantName Name { get; private set; } = null!;
    public TenantIdentifier TenantIdentifier { get; private set; } = null!;

    public void SetName(TenantName name)
    {
        Name = name;
    }

    public static Tenant Provision(TenantId id, TenantName name, TenantIdentifier identifier) =>
        new(id, name, identifier);
}