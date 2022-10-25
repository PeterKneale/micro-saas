namespace Demo.Infrastructure.Tenancy;

internal interface ISetTenantContext
{
    void SetCurrentTenant(string tenant);
}