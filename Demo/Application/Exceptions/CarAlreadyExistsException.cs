namespace Demo.Application.Exceptions;

public class CarAlreadyExistsException : Exception
{
    public CarAlreadyExistsException(Guid id) : base($"Car {id} already exists")
    {
    }
}