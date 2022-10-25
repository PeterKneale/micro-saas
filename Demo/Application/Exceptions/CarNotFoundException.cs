namespace Demo.Application.Exceptions;

public class CarNotFoundException : Exception
{
    public CarNotFoundException(Guid id) : base($"Car {id} not found")
    {
    }
    public CarNotFoundException(string registration) : base($"Car {registration} not found")
    {
    }
}