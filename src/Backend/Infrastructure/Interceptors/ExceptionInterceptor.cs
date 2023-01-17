using Backend.Infrastructure.ExecutionContext;
using Backend.Modules.Exceptions;
using Grpc.Core.Interceptors;
using ValidationException = FluentValidation.ValidationException;

namespace Backend.Infrastructure.Interceptors;

public class ExceptionInterceptor : Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _log;

    public ExceptionInterceptor(ILogger<ExceptionInterceptor> log)
    {
        _log = log;
    }
    
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        _log.LogInformation("Monitoring grpc pipeline for exceptions");
        try
        {
            return await continuation(request, context);
        }
        catch (ValidationException ex)
        {
            _log.LogError(ex, "Validation exception detected, returning InvalidArgument");
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (NotFoundException ex)
        {
            _log.LogError(ex, "Not found exception detected, returning NotFound");
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (AlreadyExistsException ex)
        {
            _log.LogError(ex, "Already exists exception detected, returning NotFound");
            throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
        }
        catch (RuleBrokenException ex)
        {
            _log.LogError(ex, "Rule broken exception detected, returning FailedPrecondition");
            throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
        }
        catch (ExecutionContextNotAvailableException ex)
        {
            _log.LogError(ex, "TenantContext not available, returning PermissionDenied");
            throw new RpcException(new Status(StatusCode.PermissionDenied, ex.Message));
        }
        catch (Exception e)
        {
            _log.LogError(e, "Unknown exception detected, throwing");
            throw;
        }
    }
}
