namespace Backend.Modules.Exceptions;

public abstract class RuleBrokenException : Exception
{
    protected RuleBrokenException(string message) : base(message)
    {
    }
}