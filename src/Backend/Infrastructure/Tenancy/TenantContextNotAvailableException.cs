namespace Backend.Infrastructure.Tenancy;

internal class TenantContextNotAvailableException : Exception
{
    public TenantContextNotAvailableException(string message) : base(message)
    {
        
    }
}