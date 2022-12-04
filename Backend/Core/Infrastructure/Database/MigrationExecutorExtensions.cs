namespace Backend.Core.Infrastructure.Database;

public static class MigrationExecutorExtensions
{
    public static void ExecuteDatabaseMigration(this IServiceProvider provider, Action<MigrationExecutor> action)
    {
        using var scope = provider.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<MigrationExecutor>();
        action(migrator);
    }
}