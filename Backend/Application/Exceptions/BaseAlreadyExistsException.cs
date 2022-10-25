namespace Backend.Application.Exceptions;

internal abstract class BaseAlreadyExistsException : Exception
{
    protected BaseAlreadyExistsException(string type, string id) : base($"{type} {id} already exists")
    {
    }
}