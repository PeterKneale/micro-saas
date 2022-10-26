namespace Backend.Domain.TenantAggregate;

public class Tenant
{
    private Tenant(TenantId id, Name name, Identifier identifier)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
    }

    private Tenant()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public TenantId Id { get; private set; } = null!;
    public Name Name { get; private set; } = null!;
    public Identifier Identifier { get; private set; } = null!;

    public void SetName(Name name)
    {
        Name = name;
    }

    public static Tenant CreateInstance(TenantId id, Name name, Identifier identifier) =>
        new(id, name, identifier);
}