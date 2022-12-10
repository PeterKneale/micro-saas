﻿namespace Backend.Features.Tenancy.Domain.TenantAggregate;

public class Tenant
{
    private Tenant(TenantId id, TenantName name, Identifier identifier)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
    }

    private Tenant()
    {
        // Parameterless constructor for serialisation
    }

    // Private setter for serialisation
    public TenantId Id { get; private set; } = null!;
    public TenantName Name { get; private set; } = null!;
    public Identifier Identifier { get; private set; } = null!;

    public void SetName(TenantName name)
    {
        Name = name;
    }

    public static Tenant Provision(TenantId id, TenantName name, Identifier identifier) =>
        new(id, name, identifier);
}