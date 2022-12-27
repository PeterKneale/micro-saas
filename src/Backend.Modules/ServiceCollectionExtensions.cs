using System.Reflection;
using Backend.Modules.Infrastructure.Behaviours;
using Backend.Modules.Infrastructure.Database;
using Backend.Modules.Infrastructure.Emails;
using FluentMigrator.Runner;

namespace Backend.Modules;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        // Update mediatr request pipeline
        services
            // Log the request
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
            // Validate the request
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>))
            // Open a connection, begin a transaction and set the tenant context for the connection
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TenantConnectionBehaviour<,>)); 

        // Update database connections and repositories
        // Note that there is a single connection per request as this is a scoped dependency
        // This is important as this way the tenant context set by the TenantContextBehaviour
        // is the same one used by the repository
        services
            .AddScoped<IAdminConnectionFactory, AdminConnectionFactory>()
            .AddScoped<ITenantConnectionFactory, TenantConnectionFactory>();
        
        services
            .AddTransient<IEmailSender, EmailSender>();

        // Update tenant context
        // a single instance of tenant context is created per request
        // requests for ISet and IGet are both forwarded to the same instance 
        services
            .AddScoped<TenantContext>()
            .AddScoped<IGetTenantContext>(c => c.GetRequiredService<TenantContext>())
            .AddScoped<ISetTenantContext>(c => c.GetRequiredService<TenantContext>());

        // Update database migrations and executor
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(configuration.GetSystemConnectionString())
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
            .AddScoped<MigrationExecutor>();

        return services;
    }
}