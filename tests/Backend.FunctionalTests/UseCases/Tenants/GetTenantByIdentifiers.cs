﻿using Backend.FunctionalTests.Fixtures;

namespace Backend.FunctionalTests.UseCases.Tenants;

[Collection(nameof(ServiceCollectionFixture))]
public class GetTenantByIdentifiers
{
    private readonly TenantsApi.TenantsApiClient _adminClient;

    public GetTenantByIdentifiers(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _adminClient = service.TenantAdminClient;
    }

    [Fact]
    public async Task CanGetTenantByIdentifier()
    {
        // arrange
        var id = Guid.NewGuid().ToString();
        var identifier = Guid.NewGuid().ToString();
        var name = Guid.NewGuid().ToString();

        // act
        await _adminClient.AddTenantAsync(new AddTenantRequest
        {
            Id = id,
            Identifier = identifier,
            Name = name
        });
        var result = await _adminClient.GetTenantByIdentifierAsync(new GetTenantByIdentifierRequest
        {
            Identifier = identifier
        });

        // assert
        result.Id.Should().Be(id);
        result.Identifier.Should().Be(identifier);
        result.Name.Should().Be(name);
    }
}