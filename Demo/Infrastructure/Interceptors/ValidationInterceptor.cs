using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Demo.Infrastructure.Interceptors;

internal class ValidationInterceptor : Interceptor
{
    private readonly IServiceProvider _services;
    private readonly ILogger<ValidationInterceptor> _log;

    public ValidationInterceptor(IServiceProvider services, ILogger<ValidationInterceptor> log)
    {
        _services = services;
        _log = log;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var validator = _services.GetService<IValidator<TRequest>>();
        if (validator != null)
        {
            _log.LogWarning("Validators {Validator} found for {Request}", validator.GetType().Name, typeof(TRequest).Name);
            await validator.ValidateAndThrowAsync(request, context.CancellationToken);
            _log.LogWarning("Validators success for {Request}", typeof(TRequest).Name);
        }
        else
        {
            _log.LogWarning("No validators could be found for {Request}", typeof(TRequest).Name);
        }
        return await continuation(request, context);
    }
}