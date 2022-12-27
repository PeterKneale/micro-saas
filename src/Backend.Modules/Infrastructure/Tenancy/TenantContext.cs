namespace Backend.Modules.Infrastructure.Tenancy;

internal class TenantContext : IGetTenantContext, ISetTenantContext
{
    private Guid? _tenant;

    public Guid CurrentTenant =>
        _tenant ?? throw new EmptyTenantContextException();

    public void SetCurrentTenant(Guid tenant)
    {
        _tenant = tenant;
    }
}