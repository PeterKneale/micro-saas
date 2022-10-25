namespace Backend.Application.Exceptions;

internal class TenantBaseNotFoundException : BaseNotFoundException
{
    public TenantBaseNotFoundException(Guid id) : base("tenant", id.ToString())
    {
    }
    public TenantBaseNotFoundException(string name) : base("tenant", name)
    {
    }
}