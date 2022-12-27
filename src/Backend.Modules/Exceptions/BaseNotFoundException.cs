namespace Backend.Modules.Exceptions;

public abstract class BaseNotFoundException : Exception
{
    protected BaseNotFoundException(string type, string id) : base($"{type} {id} not found")
    {
    }
}