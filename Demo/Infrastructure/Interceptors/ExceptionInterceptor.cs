using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Demo.Infrastructure.Interceptors;

internal class ExceptionInterceptor : Interceptor
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
        catch (CarNotFoundException ex)
        {
            _log.LogError(ex, "Not found exception detected, returning NotFound");
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (CarAlreadyExistsException ex)
        {
            _log.LogError(ex, "Already exists exception detected, returning NotFound");
            throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
        }
        catch (CarAlreadyRegisteredException ex)
        {
            _log.LogError(ex, "Car already registered exception detected, returning FailedPrecondition");
            throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
        }
        catch (RegistrationAlreadyExistsException ex)
        {
            _log.LogError(ex, "Registration already exists exception detected, returning AlreadyExists");
            throw new RpcException(new Status(StatusCode.AlreadyExists, ex.Message));
        }
        catch (MissingTenantHeaderException ex)
        {
            _log.LogError(ex, "Missing tenant header detected, returning PermissionDenied");
            throw new RpcException(new Status(StatusCode.PermissionDenied, ex.Message));
        }
        catch (Exception e)
        {
            _log.LogError(e, "Unknown exception detected, throwing");
            throw;
        }
    }
}
