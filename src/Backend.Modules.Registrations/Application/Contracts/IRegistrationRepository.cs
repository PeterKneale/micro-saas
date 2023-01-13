﻿using Backend.Modules.Registrations.Domain.Common;
using Backend.Modules.Registrations.Domain.RegistrationAggregate;

namespace Backend.Modules.Registrations.Application.Contracts;

internal interface IRegistrationRepository
{
    Task Insert(Domain.RegistrationAggregate.Registration registration, CancellationToken cancellationToken);
    
    Task Update(Domain.RegistrationAggregate.Registration registration, CancellationToken cancellationToken);
    
    Task<Domain.RegistrationAggregate.Registration?> Get(RegistrationId id, CancellationToken cancellationToken);
    
    Task<IEnumerable<Domain.RegistrationAggregate.Registration>> Get(TenantIdentifier identifier, CancellationToken cancellationToken);

    Task<IEnumerable<Domain.RegistrationAggregate.Registration>> List(CancellationToken cancellationToken);
}