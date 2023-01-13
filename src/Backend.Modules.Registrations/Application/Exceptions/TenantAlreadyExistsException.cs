using Backend.Modules.Registrations.Domain.Common;

namespace Backend.Modules.Registrations.Application.Exceptions;

internal class TenantAlreadyExistsException : AlreadyExistsException
{
    public TenantAlreadyExistsException(TenantId id) : base("tenant", id.Id.ToString())
    {
    }
}