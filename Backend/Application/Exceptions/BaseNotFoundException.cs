namespace Backend.Application.Exceptions;

internal abstract class BaseNotFoundException : Exception
{
    protected BaseNotFoundException(string type, string id) : base($"{type} {id} not found")
    {
    }
}