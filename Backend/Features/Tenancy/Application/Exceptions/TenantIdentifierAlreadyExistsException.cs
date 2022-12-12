using Backend.Core.Exceptions;
using Backend.Features.Tenancy.Domain.TenantAggregate;

namespace Backend.Features.Tenancy.Application.Exceptions;

internal class TenantIdentifierAlreadyExistsException : BaseAlreadyExistsException
{
    public TenantIdentifierAlreadyExistsException(TenantIdentifier identifier) : base("tenant", identifier.Value)
    {
    }
}