namespace Backend.Modules.Tenants.Domain.RegistrationAggregate;

public class RegistrationId
{
    private RegistrationId(Guid id)
    {
        Id = id;
    }

    private RegistrationId()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public Guid Id { get; private set; }

    public static RegistrationId CreateInstance(Guid? id = null)
    {
        return new RegistrationId(id ?? Guid.NewGuid());
    }
}