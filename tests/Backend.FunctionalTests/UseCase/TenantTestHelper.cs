using Grpc.Core;

namespace Backend.FunctionalTests.UseCase;

public class TenantTestHelper
{
    private readonly TenantsApi.TenantsApiClient _admin;

    public TenantTestHelper(TenantsApi.TenantsApiClient admin)
    {
        _admin = admin;
    }
    public async Task<Metadata> CreateTenant()
    {
        var id = Guid.NewGuid().ToString();
        var identifier = Guid.NewGuid().ToString();
        var name = Guid.NewGuid().ToString();

        await _admin.AddTenantAsync(new AddTenantRequest
        {
            Id = id,
            Identifier = identifier,
            Name = name
        });
        return MetaDataBuilder.WithTenant(id);
    }
}