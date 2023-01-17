using Modules.Infrastructure.Repositories.Serialisation;
using Modules.Registrations.Application.Contracts;
using Modules.Registrations.Domain.Common;
using Modules.Registrations.Domain.RegistrationAggregate;
using static Modules.Registrations.Infrastructure.Database.Constants;

namespace Modules.Registrations.Infrastructure.Repositories;

internal class RegistrationRepository : IRegistrationRepository
{
    private readonly IDbConnection _connection;

    public RegistrationRepository(IAdminConnectionFactory factory)
    {
        _connection = factory.GetDbConnectionForAdmin();
    }

    public async Task Insert(Domain.RegistrationAggregate.Registration registration, CancellationToken cancellationToken)
    {
        const string sql = $"insert into {Schema}.{TableRegistrations} ({ColumnId},  {ColumnRegistrationIdentifier}, {ColumnData}) values (@id, @identifier,@data::jsonb)";
        var json = JsonHelper.ToJson(registration);
        await _connection.ExecuteAsync(sql, new
        {
            id = registration.Id.Id,
            identifier = registration.Identifier.Value,
            data = json
        });
    }

    public async Task Update(Domain.RegistrationAggregate.Registration registration, CancellationToken cancellationToken)
    {
        const string sql = $"update {Schema}.{TableRegistrations} set {ColumnData} = @data::jsonb where {ColumnId} = @id";
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

    public async Task<IEnumerable<Domain.RegistrationAggregate.Registration>> Get(TenantIdentifier identifier, CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {Schema}.{TableRegistrations} where {ColumnRegistrationIdentifier} = @identifier";
        var results = await _connection.QueryAsync<string>(sql, new
        {
            identifier = identifier.Value
        });
        return results.Select(x => JsonHelper.ToObject<Domain.RegistrationAggregate.Registration>(x)!);
    }

    public async Task<Domain.RegistrationAggregate.Registration?> Get(RegistrationId id, CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {Schema}.{TableRegistrations} where {ColumnId} = @id";
        var result = await _connection.QuerySingleOrDefaultAsync<string>(sql, new
        {
            id = id.Id
        });
        return JsonHelper.ToObject<Domain.RegistrationAggregate.Registration>(result);
    }

    public async Task<IEnumerable<Domain.RegistrationAggregate.Registration>> List(CancellationToken cancellationToken)
    {
        const string sql = $"select {ColumnData} from {Schema}.{TableRegistrations}";
        var results = await _connection.QueryAsync<string>(sql, cancellationToken);
        return results
            .Select(result => JsonHelper.ToObject<Domain.RegistrationAggregate.Registration>(result)!)
            .ToList();
    }
}