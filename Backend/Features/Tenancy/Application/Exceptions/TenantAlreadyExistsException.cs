using Backend.Core.Exceptions;

namespace Backend.Features.Tenancy.Application.Exceptions;

internal class TenantAlreadyExistsException : BaseAlreadyExistsException
{
    public TenantAlreadyExistsException(Guid id) : base("tenant", id.ToString())
    {
    }
}