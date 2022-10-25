namespace Demo.Application.Exceptions;

public class CarAlreadyRegisteredException : Exception
{
    public CarAlreadyRegisteredException(string registration) : base($"Car {registration} already registered")
    {
    }
}