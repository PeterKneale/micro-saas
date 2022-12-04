using Polly;
using Polly.Retry;

namespace Backend.Core.Infrastructure.Database;

public class MigrationExecutor
{
    private readonly IMigrationRunner _runner;
    private readonly ILogger<MigrationExecutor> _log;
    private readonly RetryPolicy _policy;

    public MigrationExecutor(IMigrationRunner runner, ILogger<MigrationExecutor> log)
    {
        _runner = runner;
        _log = log;

        var attempts = 30;
        var delay = TimeSpan.FromSeconds(2);
        
        _policy = Policy
            .Handle<Exception>()
            .WaitAndRetry(attempts, _ => delay, (exception, timeSpan, retryCount, context) =>
            {
                _log.LogWarning(exception, "Failed to connect to database. Retrying in {Delay} seconds. Attempt {RetryCount} of {RetryAttempts}", delay.TotalSeconds, retryCount, attempts);
            });
    }

    public void ApplyDatabaseMigrations()
    {
        _policy.Execute(() => _runner.MigrateUp());
    }

    public void ResetDatabase()
    {
        _policy.Execute(() => {
            _runner.MigrateDown(0);
            _runner.MigrateUp();
        });
    }
}