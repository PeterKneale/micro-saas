namespace Backend.Core.Infrastructure.Tenancy;

internal interface ISetTenantContext
{
    void SetCurrentTenant(Guid tenant);
}