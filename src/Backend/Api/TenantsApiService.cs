using Backend.Modules.Tenants;
using Backend.Modules.Tenants.Application.Commands;
using Backend.Modules.Tenants.Application.Queries;

namespace Backend.Api;

public class TenantsApiService : TenantsApi.TenantsApiBase
{
    private readonly ITenantsModule _module;

    public TenantsApiService(ITenantsModule module)
    {
        _module = module;
    }

    public override async Task<EmptyResponse2> AddTenant(AddTenantRequest request, ServerCallContext context)
    {
        await _module.ExecuteCommandAsync(new AddTenant.Command(Guid.Parse(request.Id), request.Name, request.Identifier));
        return new EmptyResponse2();
    }

    public override async Task<GetTenantResponse> GetTenant(GetTenantRequest request, ServerCallContext context)
    {
        var result = await _module.ExecuteQueryAsync(new GetTenant.Query(Guid.Parse(request.Id)));
        return new GetTenantResponse
        {
            Id = result.Id.ToString(),
            Name = result.Name,
            Identifier = result.Identifier
        };
    }

    public override async Task<GetTenantByIdentifierResponse> GetTenantByIdentifier(GetTenantByIdentifierRequest request, ServerCallContext context)
    {
        var result = await _module.ExecuteQueryAsync(new GetTenantByIdentifier.Query(request.Identifier));
        return new GetTenantByIdentifierResponse
        {
            Id = result.Id.ToString(),
            Name = result.Name,
            Identifier = result.Identifier
        };
    }

    public override async Task<ListTenantsResponse> ListTenants(ListTenantsRequest request, ServerCallContext context)
    {
        var results = await _module.ExecuteQueryAsync(new ListTenants.Query());
        var items = results.Select(x => new ListTenantsItem
        {
            Id = x.Id.ToString(),
            Name = x.Name,
            Identifier = x.Identifier
        });
        return new ListTenantsResponse
        {
            Items = {items}
        };
    }
}