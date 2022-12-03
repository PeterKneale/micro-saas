using Backend.Core.Exceptions;

namespace Backend.Features.Tenancy.Application.Exceptions;

internal class TenantNotFoundException : BaseNotFoundException
{
    public TenantNotFoundException(Guid id) : base("tenant", id.ToString())
    {
    }
    public TenantNotFoundException(string name) : base("tenant", name)
    {
    }
}