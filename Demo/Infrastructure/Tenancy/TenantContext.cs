namespace Demo.Infrastructure.Tenancy;

internal class TenantContext : IGetTenantContext, ISetTenantContext
{
    private string? _tenant;

    public string CurrentTenant =>
        _tenant ?? throw new EmptyTenantContextException();

    public void SetCurrentTenant(string tenant)
    {
        _tenant = tenant;
    }
}