﻿using Backend.Modules.Registrations.Domain.Common;

namespace Backend.Modules.Registrations.Application.Exceptions;

internal class TenantIdentifierAlreadyExistsException : AlreadyExistsException
{
    public TenantIdentifierAlreadyExistsException(TenantIdentifier identifier) : base("tenant", identifier.Value)
    {
    }
}