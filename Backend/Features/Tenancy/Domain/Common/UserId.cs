namespace Backend.Features.Tenancy.Domain.Common;

public class UserId
{
    private UserId(Guid id)
    {
        Id = id;
    }

    private UserId()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public Guid Id { get; private set; }

    public static UserId CreateInstance(Guid? id = null)
    {
        return new UserId(id ?? Guid.NewGuid());
    }
}