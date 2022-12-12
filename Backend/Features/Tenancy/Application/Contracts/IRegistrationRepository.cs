using Backend.Features.Tenancy.Domain.RegistrationAggregate;

namespace Backend.Features.Tenancy.Application.Contracts;

internal interface IRegistrationRepository
{
    Task Insert(Registration registration, CancellationToken cancellationToken);
    
    Task Update(Registration registration, CancellationToken cancellationToken);
    
    Task<Registration?> Get(RegistrationId id, CancellationToken cancellationToken);
    
    Task<IEnumerable<Registration>> Get(TenantIdentifier identifier, CancellationToken cancellationToken);

    Task<IEnumerable<Registration>> List(CancellationToken cancellationToken);
}