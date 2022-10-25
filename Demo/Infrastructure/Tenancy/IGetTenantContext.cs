namespace Demo.Infrastructure.Tenancy;

internal interface IGetTenantContext
{
    string CurrentTenant { get; }
}