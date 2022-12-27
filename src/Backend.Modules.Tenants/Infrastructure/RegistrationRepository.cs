using Backend.Modules.Infrastructure.Repositories.Serialisation;
using Backend.Modules.Tenants.Application.Contracts;
using Backend.Modules.Tenants.Domain.Common;
using Backend.Modules.Tenants.Domain.RegistrationAggregate;
using static Backend.Modules.Infrastructure.Database.Constants;

namespace Backend.Modules.Tenants.Infrastructure;

internal class RegistrationRepository : IRegistrationRepository
{
    private readonly IDbConnection _connection;

    public RegistrationRepository(IAdminConnectionFactory factory)
    {
        _connection = factory.GetDbConnectionForAdmin();
    }

    public async Task Insert(Registration registration, CancellationToken cancellationToken)
    {
        const string sql = $"insert into {TableRegistrations} ({ColumnId},  {ColumnRegistrationIdentifier}, {ColumnData}) values (@id, @identifier,@data::jsonb)";
        var json = JsonHelper.ToJson(registration);
        await _connection.ExecuteAsync(sql, new
        {
            id = registration.Id.Id,
            identifier = registration.Identifier.Value,
            data = json
        });
    }

    public async Task Update(Registration registration, CancellationToken cancellationToken)
    {
        const string sql = $"update {TableRegistrations} set {ColumnData} = @data::jsonb where {ColumnId} = @id";
        var result = await _connection.ExecuteAsync(sql, new
        {
            id = registration.Id.Id,
            data = JsonHelper.ToJson(registration)
        });
        if (result != 1)
        {
            throw new Exception("Record not updated");
        }
    }

    public async Task<IEnumerable<Registration>> Get(TenantIdentifier identifier, CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {TableRegistrations} where {ColumnRegistrationIdentifier} = @identifier";
        var results = await _connection.QueryAsync<string>(sql, new
        {
            identifier = identifier.Value
        });
        return results.Select(x => JsonHelper.ToObject<Registration>(x)!);
    }

    public async Task<Registration?> Get(RegistrationId id, CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {TableRegistrations} where {ColumnId} = @id";
        var result = await _connection.QuerySingleOrDefaultAsync<string>(sql, new
        {
            id = id.Id
        });
        return JsonHelper.ToObject<Registration>(result);
    }

    public async Task<IEnumerable<Registration>> List(CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {TableRegistrations}";
        var results = await _connection.QueryAsync<string>(sql, cancellationToken);
        return results
            .Select(result => JsonHelper.ToObject<Registration>(result)!)
            .ToList();
    }
}