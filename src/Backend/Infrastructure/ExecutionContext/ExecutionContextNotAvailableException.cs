namespace Backend.Infrastructure.ExecutionContext;

internal class ExecutionContextNotAvailableException : Exception
{
    public ExecutionContextNotAvailableException(string message) : base(message)
    {
        
    }
}