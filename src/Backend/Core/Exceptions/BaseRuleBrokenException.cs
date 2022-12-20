namespace Backend.Core.Exceptions;

internal abstract class BaseRuleBrokenException : Exception
{
    protected BaseRuleBrokenException(string message) : base(message)
    {
    }
}