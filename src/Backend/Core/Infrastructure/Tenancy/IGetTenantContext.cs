namespace Backend.Core.Infrastructure.Tenancy;

internal interface IGetTenantContext
{
    Guid CurrentTenant { get; }
}