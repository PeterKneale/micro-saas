namespace Backend.Domain.TenantAggregate;

public class TenantId
{
    private TenantId(Guid id)
    {
        Id = id;
    }

    private TenantId()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public Guid Id { get; private set; }

    public static TenantId CreateInstance(Guid? id = null)
    {
        return new TenantId(id ?? Guid.NewGuid());
    }
}