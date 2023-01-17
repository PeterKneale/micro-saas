using Modules.Registrations.Domain.Common;

namespace Modules.Registrations.Application.Exceptions;

internal class TenantAlreadyExistsException : AlreadyExistsException
{
    public TenantAlreadyExistsException(TenantId id) : base("tenant", id.Id.ToString())
    {
    }
}