namespace Backend.Features.Tenancy.Application.Contracts;

internal interface ITenantStatisticsRepository
{
    Task<int> CountTenants(CancellationToken cancellationToken);
    Task<int> CountWidgets(CancellationToken cancellationToken);
}