using Grpc.Core;

namespace Backend.FunctionalTests.UseCase;

public class TenantTestHelper
{
    private readonly TenantAdminService.TenantAdminServiceClient _admin;

    public TenantTestHelper(TenantAdminService.TenantAdminServiceClient admin)
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