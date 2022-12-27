namespace Backend.Modules.Exceptions;

public abstract class BaseAlreadyExistsException : Exception
{
    protected BaseAlreadyExistsException(string type, string id) : base($"{type} {id} already exists")
    {
    }
}