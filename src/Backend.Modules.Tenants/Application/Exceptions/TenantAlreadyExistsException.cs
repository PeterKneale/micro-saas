using Backend.Modules.Tenants.Domain.Common;

namespace Backend.Modules.Tenants.Application.Exceptions;

internal class TenantAlreadyExistsException : AlreadyExistsException
{
    public TenantAlreadyExistsException(TenantId id) : base("tenant", id.Id.ToString())
    {
    }
}