﻿namespace Modules.Tenants.Application.Exceptions;

internal class TenantAlreadyExistsException : AlreadyExistsException
{
    public TenantAlreadyExistsException(TenantId id) : base("tenant", id.Id.ToString())
    {
    }
}