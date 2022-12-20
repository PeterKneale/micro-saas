using Backend.Features.Tenancy.Domain.SettingsAggregate;
using Backend.Features.Tenancy.Domain.TenantAggregate;

namespace Backend.Features.Tenancy.Application.Contracts;

internal interface ISettingsRepository
{
    Task Insert(Settings settings, CancellationToken cancellationToken);
    Task Update(Settings settings, CancellationToken cancellationToken);
    
    Task<Settings?> Get(CancellationToken cancellationToken);
}