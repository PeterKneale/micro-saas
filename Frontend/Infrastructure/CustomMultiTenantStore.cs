using Backend.Api;
using Finbuckle.MultiTenant;

namespace Frontend.Infrastructure;

public class CustomMultiTenantStore : IMultiTenantStore<TenantInfo>
{
    private readonly AdminService.AdminServiceClient _client;
    private readonly ILogger<CustomMultiTenantStore> _log;

    public CustomMultiTenantStore(AdminService.AdminServiceClient client, ILogger<CustomMultiTenantStore> log)
    {
        _client = client;
        _log = log;
    }
    
    public async Task<TenantInfo?> TryGetByIdentifierAsync(string identifier)
    {
        try
        {
            _log.LogInformation("Attempting to identify tenant {Identifier}", identifier);
            var result = await GetByIdentifierAsync(identifier);
            _log.LogInformation("Identified tenant {TenantId}", result!.Id);
            return result;
        }
        catch (Exception e)
        {
            _log.LogError(e, "Failed to identify tenant {Identifier} ", identifier);
            return null;
        }
    }
    private async Task<TenantInfo?> GetByIdentifierAsync(string identifier)
    {
        var request = new GetTenantByIdentifierRequest {Identifier = identifier};
        var response = await _client.GetTenantByIdentifierAsync(request);
        return new TenantInfo
        {
            Id = response.Id,
            Identifier = request.Identifier,
            Name = response.Name
        };
    }
    
    public Task<TenantInfo?> TryGetAsync(string id) => throw new NotImplementedException();
    public Task<bool> TryAddAsync(TenantInfo tenantInfo) => throw new NotImplementedException();
    public Task<bool> TryUpdateAsync(TenantInfo tenantInfo) => throw new NotImplementedException();
    public Task<bool> TryRemoveAsync(string tenantInfo) => throw new NotImplementedException();
    public Task<IEnumerable<TenantInfo>> GetAllAsync() => throw new NotImplementedException();
}