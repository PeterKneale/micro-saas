using Demo.Api;
using Demo.Application.Contracts;
using Demo.Infrastructure.Behaviours;
using Demo.Infrastructure.Configuration;
using Demo.Infrastructure.Interceptors;
using Demo.Infrastructure.Tenancy;

namespace Demo;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDemo(this IServiceCollection services, IConfiguration configuration)
    {
        // Register libraries
        var assembly = Assembly.GetExecutingAssembly();
        services
            .AddMediatR(assembly)
            .AddValidatorsFromAssembly(assembly);

        // Register grpc request pipeline
        services
            // Add common interceptors for both the admin and tenant api's
            .AddGrpc(options => {
                // Start trapping for exceptions and translate to grpc status codes
                options.Interceptors.Add<ExceptionInterceptor>();       
                // Validate the grpc request
                options.Interceptors.Add<ValidationInterceptor>();      
            })
            .AddServiceOptions<TenantApi>(options => {
                // Capture the tenant context from grpc metadata
                options.Interceptors.Add<TenantContextInterceptor>();   
            });

        // Register mediatr request pipeline
        services
            // Log the request
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
            // Open a connection, begin a transaction and set the tenant context for the connection
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TenantConnectionBehaviour<,>)); 

        // Register database connections and repositories
        // Note that there is a single connection per request as this is a scoped dependency
        // This is important as this way the tenant context set by the TenantContextBehaviour
        // is the same one used by the repository
        services
            .AddScoped<IConnectionFactory,ConnectionFactory>()
            .AddScoped<ICarRepository, CarRepository>()
            .AddScoped<IAdminRepository, AdminRepository>();

        // Register tenant context
        // a single instance of tenant context is created per request
        // requests for ISet and IGet are both forwarded to the same instance 
        services
            .AddScoped<TenantContext>()
            .AddScoped<IGetTenantContext>(c => c.GetRequiredService<TenantContext>())
            .AddScoped<ISetTenantContext>(c => c.GetRequiredService<TenantContext>());

        // Register database migrations and executor
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(configuration.GetSystemConnectionString())
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());
        services
            .AddScoped<MigrationExecutor>();

        return services;
    }
}