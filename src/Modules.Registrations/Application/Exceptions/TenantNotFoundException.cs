namespace Modules.Registrations.Application.Exceptions;

internal class TenantNotFoundException : NotFoundException
{
    public TenantNotFoundException(Guid id) : base("tenant", id.ToString())
    {
    }
    
    public TenantNotFoundException(string name) : base("tenant", name)
    {
    }
}