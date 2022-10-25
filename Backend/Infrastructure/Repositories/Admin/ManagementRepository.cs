using Backend.Application.Contracts.Admin;
using Backend.Domain.TenantAggregate;
using Backend.Infrastructure.Repositories.Serialisation;
using Dapper;

namespace Backend.Infrastructure.Repositories.Admin;

internal class ManagementRepository : IManagementRepository
{
    private readonly IDbConnection _connection;

    public ManagementRepository(IConnectionFactory factory)
    {
        _connection = factory.GetDbConnectionForAdmin();
    }
    
    public async Task Insert(Tenant tenant, CancellationToken cancellationToken)
    {
        const string sql = "insert into tenants (id, tenant, data) values (@id, @tenant, @data::jsonb)";
        var json = JsonHelper.ToJson(tenant);
        await _connection.ExecuteAsync(sql, new
        {
            id = tenant.Id.Id,
            tenant = tenant.Id.Id.ToString(),
            data = json
        });
    }
    
    public async Task<Tenant?> Get(TenantId tenantId, CancellationToken cancellationToken)
    {
        const string sql = "select data from tenants where id = @id";
        var result = await _connection.QuerySingleOrDefaultAsync<string>(sql, new
        {
            id = tenantId.Id
        });
        return JsonHelper.ToObject<Tenant>(result);
    }
    
    public async Task<IEnumerable<Tenant>> ListTenants(CancellationToken cancellationToken)
    {
        const string sql = $"select data from tenants";
        var results = await _connection.QueryAsync<string>(sql, cancellationToken);
        return results
            .Select(result => JsonHelper.ToObject<Tenant>(result)!)
            .ToList();
    }
}