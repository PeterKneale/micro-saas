namespace Modules.Infrastructure.Behaviours;

internal class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IServiceProvider _services;
    private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _log;

    public ValidationBehaviour(IServiceProvider services, ILogger<ValidationBehaviour<TRequest, TResponse>> log)
    {
        _services = services;
        _log = log;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validator = _services.GetService<IValidator<TRequest>>();
        if (validator != null)
        {
            _log.LogWarning("Validators {Validator} found for {Request}", validator.GetType().Name, typeof(TRequest).Name);
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            _log.LogWarning("Validators success for {Request}", typeof(TRequest).Name);
        }
        else
        {
            _log.LogWarning("No validators could be found for {Request}", typeof(TRequest).Name);
        }

        return await next();
    }
}