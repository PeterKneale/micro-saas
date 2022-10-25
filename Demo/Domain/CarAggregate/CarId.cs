namespace Demo.Domain.CarAggregate;

public class CarId
{
    private CarId(Guid id)
    {
        Id = id;
    }

    private CarId()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public Guid Id { get; private set; }

    public static CarId CreateInstance(Guid? id = null)
    {
        return new CarId(id ?? Guid.NewGuid());
    }
}