using FluentMigrator.Runner;

namespace Backend.Modules.Infrastructure.Database;

public class MigrationExecutor
{
    private readonly IMigrationRunner _runner;
    private readonly ILogger<MigrationExecutor> _log;
    private readonly RetryPolicy _policy;

    public MigrationExecutor(IMigrationRunner runner, ILogger<MigrationExecutor> log)
    {
        _runner = runner;
        _log = log;

        var attempts = 1;
        var delay = TimeSpan.FromSeconds(1);

        _policy = Policy
            .Handle<Exception>()
            .WaitAndRetry(attempts, _ => delay, (exception, timeSpan, retryCount, context) =>
            {
                _log.LogWarning("Failed to connect to database. {Message} Retrying in {Delay} seconds. Attempt {RetryCount} of {RetryAttempts}", 
                    exception.Message, delay.TotalSeconds, retryCount, attempts);
            });
    }

    public void ApplyDatabaseMigrations()
    {
        _log.LogInformation("Applying database migrations");
        _policy.Execute(() => _runner.MigrateUp());
        _log.LogInformation("Applied database migrations");
    }

    public void ResetDatabase()
    {
        _policy.Execute(() =>
        {
            _runner.MigrateDown(0);
            _runner.MigrateUp();
        });
    }
}