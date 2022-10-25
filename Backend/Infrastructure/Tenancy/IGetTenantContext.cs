namespace Backend.Infrastructure.Tenancy;

internal interface IGetTenantContext
{
    string CurrentTenant { get; }
}