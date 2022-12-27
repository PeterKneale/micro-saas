namespace Backend.Modules.Exceptions;

public abstract class BaseRuleBrokenException : Exception
{
    protected BaseRuleBrokenException(string message) : base(message)
    {
    }
}