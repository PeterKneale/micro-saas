using System.Reflection;
using Backend.Modules.Infrastructure.Database;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Modules.Registrations.Infrastructure.Database;

public static class MigrationRunner
{
    public static void Run(string connection, Action<MigrationExecutor> action)
    {
        var services = new ServiceCollection();

        services
            .AddLogging()
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connection)
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

        services
            .AddScoped<MigrationExecutor>()
            .AddScoped(typeof(IVersionTableMetaData), typeof(MigrationVersions));;
        
        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var migrator = scope.ServiceProvider.GetRequiredService<MigrationExecutor>();
        action(migrator);
    }
}