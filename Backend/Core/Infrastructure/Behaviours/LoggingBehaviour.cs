using System.Diagnostics;
using Newtonsoft.Json;

namespace Backend.Core.Infrastructure.Behaviours;

internal class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _log;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> log)
    {
        _log = log;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).FullName;
        var requestJson = JsonConvert.SerializeObject(request, Formatting.Indented);

        _log.LogInformation("Handling {RequestName}\n{RequestJson}", requestName, requestJson);

        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();

        var responseJson = JsonConvert.SerializeObject(response, Formatting.Indented);

        _log.LogInformation("Handled {RequestName} in {ElapsedMilliseconds}ms\n{RequestJson}\n{ResponseJson}", requestName, sw.ElapsedMilliseconds, requestJson, responseJson);
        
        return response;
    }
}