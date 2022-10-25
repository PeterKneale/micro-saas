﻿using Backend.Application.Commands.Admin;
using Backend.Application.Queries.Admin;
using Backend.IntegrationTests.Fixtures;

namespace Backend.IntegrationTests.UseCase.Queries.Admin;

[Collection(nameof(ContainerCollectionFixture))]
public class ListTenantsTests
{
    private readonly IServiceProvider _provider;

    public ListTenantsTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task TenantsCanBeListed()
    {
        // arrange
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();
        var tenant1 = "A";
        var tenant2 = "B";

        // act
        await _provider.ExecuteCommand(new AddTenant.Command(tenantId1, tenant1));
        await _provider.ExecuteCommand(new AddTenant.Command(tenantId2, tenant2));
        var results = await _provider.ExecuteQuery(new ListTenants.Query());

        // assert
        results.Should().HaveCountGreaterOrEqualTo(2);
        results.Should().ContainSingle(x => x.Id == tenantId1);
        results.Should().ContainSingle(x => x.Id == tenantId2);
    }
}