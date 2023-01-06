using Backend.Modules.Infrastructure.Tenancy;
using Grpc.AspNetCore.Server;

namespace Backend.Infrastructure.Tenancy;

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
                throw new TenantContextNotAvailableException("HTTP Context is not available");
            }
            var grpcContext = httpContext.Features.Get<IServerCallContextFeature>()?.ServerCallContext;
            if (grpcContext == null)
            {
                throw new TenantContextNotAvailableException("GRPC Context is not available");
            }

            var tenantHeader = grpcContext.RequestHeaders.Get("tenant");
            if (tenantHeader == null)
            {
                throw new TenantContextNotAvailableException("Tenant Header is not available");
            }

            var value = tenantHeader.Value;
            var success = Guid.TryParse(value, out var tenantId);
            if (!success)
            {
                throw new TenantContextNotAvailableException($"Tenant Header is not valid ({value})");
            }

            return tenantId;
        }
    }
}