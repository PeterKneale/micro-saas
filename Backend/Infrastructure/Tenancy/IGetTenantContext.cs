namespace Backend.Infrastructure.Tenancy;

internal interface IGetTenantContext
{
    Guid CurrentTenant { get; }
}