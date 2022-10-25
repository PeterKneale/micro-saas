using Backend.Domain.TenantAggregate;

namespace Backend.Application.Contracts.Admin;

internal interface IManagementRepository
{
    Task Insert(Tenant tenant, CancellationToken cancellationToken);
    
    Task<Tenant?> Get(TenantId tenantId, CancellationToken cancellationToken);
    
    Task<IEnumerable<Tenant>> ListTenants(CancellationToken cancellationToken);
}