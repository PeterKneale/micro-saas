using Backend.Modules.Tenants.Domain.Common;
using Backend.Modules.Tenants.Domain.TenantAggregate;

namespace Backend.Modules.Tenants.Application.Contracts;

internal interface ITenantRepository
{
    Task Insert(Tenant tenant, CancellationToken cancellationToken);
    
    Task<Tenant?> Get(TenantId tenantId, CancellationToken cancellationToken);
    
    Task<Tenant?> Get(TenantIdentifier identifier, CancellationToken cancellationToken);
    
    Task<IEnumerable<Tenant>> ListTenants(CancellationToken cancellationToken);
}