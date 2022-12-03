using Backend.Core.Infrastructure.Repositories;
using Backend.Core.Infrastructure.Repositories.Serialisation;
using Backend.Features.Tenancy.Application.Contracts;
using Backend.Features.Tenancy.Domain.TenantAggregate;
using Dapper;
using static Backend.Core.Infrastructure.Constants;

namespace Backend.Features.Tenancy.Infrastructure;

internal class TenantRepository : ITenantRepository
{
    private readonly IDbConnection _connection;

    public TenantRepository(IAdminConnectionFactory factory)
    {
        _connection = factory.GetDbConnectionForAdmin();
    }

    public async Task Insert(Tenant tenant, CancellationToken cancellationToken)
    {
        const string sql = $"insert into {TableTenants} ({ColumnId}, {ColumnTenantName}, {ColumnTenantIdentifier}, {ColumnData}) values (@id, @name, @identifier,@data::jsonb)";
        var json = JsonHelper.ToJson(tenant);
        await _connection.ExecuteAsync(sql, new
        {
            id = tenant.Id.Id,
            name = tenant.Name.Value,
            identifier = tenant.Identifier.Value,
            data = json
        });
    }

    public async Task<Tenant?> Get(TenantId tenantId, CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {TableTenants} where {ColumnId} = @id";
        var result = await _connection.QuerySingleOrDefaultAsync<string>(sql, new
        {
            id = tenantId.Id
        });
        return JsonHelper.ToObject<Tenant>(result);
    }

    public async Task<Tenant?> Get(Identifier identifier, CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {TableTenants} where {ColumnTenantIdentifier} = @identifier";
        var result = await _connection.QuerySingleOrDefaultAsync<string>(sql, new
        {
            identifier = identifier.Value
        });
        return JsonHelper.ToObject<Tenant>(result);
    }

    public async Task<IEnumerable<Tenant>> ListTenants(CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {TableTenants}";
        var results = await _connection.QueryAsync<string>(sql, cancellationToken);
        return results
            .Select(result => JsonHelper.ToObject<Tenant>(result)!)
            .ToList();
    }
}