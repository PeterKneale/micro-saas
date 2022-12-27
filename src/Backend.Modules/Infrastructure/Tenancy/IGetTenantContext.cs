namespace Backend.Modules.Infrastructure.Tenancy;

public interface IGetTenantContext
{
    Guid CurrentTenant { get; }
}