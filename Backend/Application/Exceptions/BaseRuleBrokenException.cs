namespace Backend.Application.Exceptions;

internal abstract class BaseRuleBrokenException : Exception
{
    protected BaseRuleBrokenException(string message) : base(message)
    {
    }
}