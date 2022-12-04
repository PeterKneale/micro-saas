using Finbuckle.MultiTenant;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Frontend.Infrastructure;

public class TenantInterceptor : Interceptor
{
    private readonly IMultiTenantContext<TenantInfo> _context;
    private readonly ILogger _logger;

    public TenantInterceptor(IMultiTenantContext<TenantInfo> context, ILoggerFactory loggerFactory)
    {
        _context = context;
        _logger = loggerFactory.CreateLogger<TenantInterceptor>();
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var tenantInfo = _context.TenantInfo;
        if (tenantInfo == null)
        {
            throw new Exception("No tenant context available");
        }
        
        var tenantId = tenantInfo.Id!;
        if (tenantInfo == null)
        {
            throw new Exception("No tenant identifier available");
        }
        
        _logger.LogInformation("Appending meta data to request {Identifier}", tenantId);
        var metadata = new Metadata
        {
            {"tenant", tenantId}
        };
        
        context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, context.Options.WithHeaders(metadata));
        
        return continuation(request, context);
    }
}