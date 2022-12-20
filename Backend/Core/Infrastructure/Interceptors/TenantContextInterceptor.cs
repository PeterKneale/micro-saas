using Backend.Core.Infrastructure.Tenancy;
using Grpc.Core.Interceptors;

namespace Backend.Core.Infrastructure.Interceptors;

internal class TenantContextInterceptor : Interceptor
{
    private readonly ISetTenantContext _context;
    private readonly ILogger<TenantContextInterceptor> _log;
    private const string TenantMetaDataKey = "tenant";

    public TenantContextInterceptor(ISetTenantContext context, ILogger<TenantContextInterceptor> log)
    {
        _context = context;
        _log = log;
    }

    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _log.LogInformation("Getting tenant context from grpc headers");

        var header = context.RequestHeaders.SingleOrDefault(x => x.Key == TenantMetaDataKey);
        if (header == null)
        {
            _log.LogError("No header found");
            throw new MissingTenantHeaderException();
        }

        _log.LogInformation("Found tenant header {Tenant}", header.Value);
        var tenant = header.Value;

        if (!Guid.TryParse(tenant, out var tenantId))
        {
            _log.LogError("Invalid tenant header found {Tenant}", tenant);
            throw new MissingTenantHeaderException();
        }

        _context.SetCurrentTenant(tenantId);

        return continuation(request, context);
    }
}