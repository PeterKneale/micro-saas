﻿using Modules.Application;
using Modules.Infrastructure.Repositories;
using Modules.Infrastructure.Tenancy;

namespace Modules.Infrastructure.Behaviours;

internal class TenantConnectionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, IRequireTenantContext
{
    private readonly ITenantConnectionFactory _factory;
    private readonly IExecutionContextAccessor _contextAccessor;
    private readonly ILogger<TenantConnectionBehaviour<TRequest, TResponse>> _log;

    public TenantConnectionBehaviour(ITenantConnectionFactory factory, IExecutionContextAccessor contextAccessor, ILogger<TenantConnectionBehaviour<TRequest, TResponse>> log)
    {
        _factory = factory;
        _contextAccessor = contextAccessor;
        _log = log;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _log.LogInformation("Opening connection and beginning transaction");
        using var connection = _factory.GetDbConnectionForTenant();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        try
        {
            var currentTenant = _contextAccessor.CurrentTenant;
            _log.LogInformation("Setting the tenant context {CurrentTenant}", currentTenant);
            await connection.ExecuteAsync($"SET app.tenant_id = '{currentTenant}';");

            var result = await next();

            _log.LogInformation("Committing transaction and closing connection");
            transaction.Commit();
            connection.Close();
            return result;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Rolling back transaction");
            transaction.Rollback();
            throw;
        }
    }
}