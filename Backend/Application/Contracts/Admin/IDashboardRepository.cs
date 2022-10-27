namespace Backend.Application.Contracts.Admin;

internal interface IDashboardRepository
{
    Task<int> CountTenants(CancellationToken cancellationToken);
    Task<int> CountWidgets(CancellationToken cancellationToken);
}