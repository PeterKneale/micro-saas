namespace Backend.Modules.Exceptions;

public abstract class AlreadyExistsException : Exception
{
    protected AlreadyExistsException(string type, string id) : base($"{type} {id} already exists")
    {
    }
    protected AlreadyExistsException(string type, Guid id) : base($"{type} {id} already exists")
    {
    }
}