using Backend.Core.Infrastructure.Configuration;
using Polly;
using Polly.Retry;

namespace Backend.Core.Infrastructure.Database;

public class MigrationExecutor
{
    private readonly IMigrationRunner _runner;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MigrationExecutor> _log;
    private readonly RetryPolicy _policy;

    public MigrationExecutor(IMigrationRunner runner, IConfiguration configuration, ILogger<MigrationExecutor> log)
    {
        _runner = runner;
        _configuration = configuration;
        _log = log;

        _policy = Policy
            .Handle<Exception>()
            .WaitAndRetry(
                5,
                (x) => TimeSpan.FromSeconds(2),
                (exception, retryDelay, retryCount, context) => {
                    _log.LogWarning("Error while migrating database. Retrying in {RetryDelay} seconds. Retry {RetryCount}\n({Error})", retryDelay.TotalSeconds, retryCount, exception.Message);
                }
            );
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