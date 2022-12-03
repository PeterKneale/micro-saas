using Backend.Features.Tenancy.Domain.TenantAggregate;

namespace Backend.Features.Tenancy.Application.Contracts;

internal interface ITenantRepository
{
    Task Insert(Tenant tenant, CancellationToken cancellationToken);
    
    Task<Tenant?> Get(TenantId tenantId, CancellationToken cancellationToken);
    
    Task<Tenant?> Get(Identifier identifier, CancellationToken cancellationToken);
    
    Task<IEnumerable<Tenant>> ListTenants(CancellationToken cancellationToken);
}