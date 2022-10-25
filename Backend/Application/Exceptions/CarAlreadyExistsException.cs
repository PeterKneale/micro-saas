namespace Backend.Application.Exceptions;

internal class CarAlreadyExistsException : BaseAlreadyExistsException
{
    public CarAlreadyExistsException(Guid id) : base("car", id.ToString())
    {
    }
}