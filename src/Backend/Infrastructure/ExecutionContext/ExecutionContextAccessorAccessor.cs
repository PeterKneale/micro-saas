using Backend.Modules.Infrastructure.Tenancy;
using Grpc.AspNetCore.Server;

namespace Backend.Infrastructure.ExecutionContext;

public class ExecutionContextAccessorAccessor : IExecutionContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExecutionContextAccessorAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid CurrentTenant
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new ExecutionContextNotAvailableException("HTTP Context is not available");
            }
            var grpcContext = httpContext.Features.Get<IServerCallContextFeature>()?.ServerCallContext;
            if (grpcContext == null)
            {
                throw new ExecutionContextNotAvailableException("GRPC Context is not available");
            }

            var tenantHeader = grpcContext.RequestHeaders.Get("tenant");
            if (tenantHeader == null)
            {
                throw new ExecutionContextNotAvailableException("Tenant Header is not available");
            }

            var value = tenantHeader.Value;
            var success = Guid.TryParse(value, out var tenantId);
            if (!success)
            {
                throw new ExecutionContextNotAvailableException($"Tenant Header is not valid ({value})");
            }

            return tenantId;
        }
    }
}