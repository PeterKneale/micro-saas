﻿using Backend.Application.Contracts;
using Backend.Application.Contracts.Tenants;
using Backend.Domain.CarAggregate;
using Backend.Infrastructure.Repositories.Serialisation;
using Backend.Infrastructure.Tenancy;
using Dapper;

namespace Backend.Infrastructure.Repositories.Tenants;

internal class CarRepository : ICarRepository
{
    private readonly IDbConnection _connection;
    private readonly IGetTenantContext _context;

    public CarRepository(IConnectionFactory factory, IGetTenantContext context)
    {
        _connection = factory.GetDbConnectionForTenant();
        _context = context;
    }

    public async Task<Car?> Get(CarId carId, CancellationToken cancellationToken)
    {
        const string sql = "select data from cars where id = @id";
        var result = await _connection.QuerySingleOrDefaultAsync<string>(sql, new
        {
            id = carId.Id
        });
        return JsonHelper.ToObject<Car>(result);
    }

    public async Task<Car?> GetByRegistration(Registration registration, CancellationToken token)
    {
        const string sql = "select data from cars where registration = @registration";
        var result = await _connection.QuerySingleOrDefaultAsync<string>(sql, new
        {
            registration = registration.RegistrationNumber
        });
        return JsonHelper.ToObject<Car>(result);
    }
    
    public async Task<IEnumerable<Car>> List(CancellationToken cancellationToken)
    {
        const string sql = "select data from cars";
        var results = await _connection.QueryAsync<string>(sql, cancellationToken);
        return results
            .Select(result => JsonHelper.ToObject<Car>(result)!)
            .ToList();
    }

    public async Task Insert(Car car, CancellationToken cancellationToken)
    {
        const string sql = "insert into cars (id, tenant_id, registration, data) values (@id, @tenant_id, @registration, @data::jsonb)";
        var json = JsonHelper.ToJson(car);
        await _connection.ExecuteAsync(sql, new
        {
            id = car.Id.Id,
            tenant_id = _context.CurrentTenant,
            registration = car.Registration?.RegistrationNumber,
            data = json
        });
    }

    public async Task Update(Car car, CancellationToken cancellationToken)
    {
        const string sql = "update cars set registration = @registration, data = @data::jsonb where id = @id";
        var result = await _connection.ExecuteAsync(sql, new
        {
            id = car.Id.Id,
            registration = car.Registration?.RegistrationNumber,
            data = JsonHelper.ToJson(car)
        });
        if (result != 1)
        {
            throw new Exception("Record not updated");
        }
    }

}