using Dapper;
using Demo.Application.Contracts;
using Demo.Infrastructure.Tenancy;

namespace Demo.Infrastructure.Behaviours;

internal class TenantConnectionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, IRequireTenantContext
{
    private readonly IConnectionFactory _factory;
    private readonly IGetTenantContext _context;
    private readonly ILogger<TenantConnectionBehaviour<TRequest, TResponse>> _log;

    public TenantConnectionBehaviour(IConnectionFactory factory, IGetTenantContext context, ILogger<TenantConnectionBehaviour<TRequest, TResponse>> log)
    {
        _factory = factory;
        _context = context;
        _log = log;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _log.LogInformation("Opening connection and beginning transaction");
        using var connection = _factory.GetConnection();
        connection.Open();

        using var transaction = connection.BeginTransaction();
        {
            try
            {
                _log.LogInformation("Setting the tenant context");
                await connection.ExecuteAsync($"SET app.tenant = '{_context.CurrentTenant}';");
                
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
}