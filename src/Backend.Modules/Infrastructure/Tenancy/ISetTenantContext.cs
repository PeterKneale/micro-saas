namespace Backend.Modules.Infrastructure.Tenancy;

public interface ISetTenantContext
{
    void SetCurrentTenant(Guid tenant);
}