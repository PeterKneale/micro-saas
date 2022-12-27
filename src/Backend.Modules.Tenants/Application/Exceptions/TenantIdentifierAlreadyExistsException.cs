using Backend.Modules.Tenants.Domain.Common;

namespace Backend.Modules.Tenants.Application.Exceptions;

internal class TenantIdentifierAlreadyExistsException : BaseAlreadyExistsException
{
    public TenantIdentifierAlreadyExistsException(TenantIdentifier identifier) : base("tenant", identifier.Value)
    {
    }
}