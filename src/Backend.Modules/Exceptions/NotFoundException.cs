namespace Backend.Modules.Exceptions;

public abstract class NotFoundException : Exception
{
    protected NotFoundException(string type, string id) : base($"{type} {id} not found")
    {
    }
    protected NotFoundException(string type, Guid id) : base($"{type} {id} not found")
    {
    }
}