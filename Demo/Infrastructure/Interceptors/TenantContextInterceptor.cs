using Demo.Infrastructure.Tenancy;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Demo.Infrastructure.Interceptors;

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

        _context.SetCurrentTenant(tenant);

        return continuation(request, context);
    }
}