namespace Demo.Infrastructure.Database;

public class MigrationExecutor
{
    private readonly IMigrationRunner _runner;

    public MigrationExecutor(IMigrationRunner runner)
    {
        _runner = runner;
    }

    public void ApplyDatabaseMigrations()
    {
        _runner.MigrateUp();
    }

    public void ResetDatabase()
    {
        _runner.MigrateDown(0);
        _runner.MigrateUp();
    }
}