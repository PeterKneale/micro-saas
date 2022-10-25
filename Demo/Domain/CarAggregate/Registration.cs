namespace Demo.Domain.CarAggregate;

public class Registration
{
    private Registration(string registrationNumber)
    {
        RegistrationNumber = registrationNumber;
    }

    private Registration()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public string RegistrationNumber { get; private set; } = null!;

    public static Registration CreateInstance(string registrationNumber)
    {
        return new Registration(registrationNumber);
    }
}