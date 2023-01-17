namespace Modules.Tenants.Application.Exceptions;

internal class TenantIdentifierAlreadyExistsException : AlreadyExistsException
{
    public TenantIdentifierAlreadyExistsException(TenantIdentifier identifier) : base("tenant", identifier.Value)
    {
    }
}