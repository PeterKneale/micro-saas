using Backend.Core.Application;
using Backend.Core.Infrastructure.Tenancy;
using Backend.Features.Widgets.Application.Contracts;

namespace Backend.Core.Infrastructure.Behaviours;

internal class TenantConnectionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, IRequireTenantContext
{
    private readonly ITenantConnectionFactory _factory;
    private readonly IGetTenantContext _context;
    private readonly ILogger<TenantConnectionBehaviour<TRequest, TResponse>> _log;

    public TenantConnectionBehaviour(ITenantConnectionFactory factory, IGetTenantContext context, ILogger<TenantConnectionBehaviour<TRequest, TResponse>> log)
    {
        _factory = factory;
        _context = context;
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
            var currentTenant = _context.CurrentTenant;
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