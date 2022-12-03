﻿using Backend.Api;
using Backend.Core.Infrastructure.Behaviours;
using Backend.Core.Infrastructure.Configuration;
using Backend.Core.Infrastructure.Database;
using Backend.Core.Infrastructure.Interceptors;
using Backend.Core.Infrastructure.Repositories;
using Backend.Core.Infrastructure.Tenancy;
using Backend.Features.Tenancy.Application.Contracts;
using Backend.Features.Tenancy.Infrastructure;
using Backend.Features.Widgets.Api;
using Backend.Features.Widgets.Application.Contracts;
using Backend.Features.Widgets.Infrastructure;

namespace Backend;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackend(this IServiceCollection services, IConfiguration configuration)
    {
        // Update libraries
        var assembly = Assembly.GetExecutingAssembly();
        services
            .AddMediatR(assembly)
            .AddValidatorsFromAssembly(assembly);

        // Update grpc request pipeline
        services
            // Add common interceptors for both the admin and tenant api's
            .AddGrpc(options => {
                // Start trapping for exceptions and translate to grpc status codes
                options.Interceptors.Add<ExceptionInterceptor>();       
                // Validate the grpc request
                options.Interceptors.Add<ValidationInterceptor>();      
            })
            .AddServiceOptions<WidgetApi>(options => {
                // Capture the tenant context from grpc metadata
                options.Interceptors.Add<TenantContextInterceptor>();   
            });

        // Update mediatr request pipeline
        services
            // Log the request
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>))
            // Open a connection, begin a transaction and set the tenant context for the connection
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TenantConnectionBehaviour<,>)); 

        // Update database connections and repositories
        // Note that there is a single connection per request as this is a scoped dependency
        // This is important as this way the tenant context set by the TenantContextBehaviour
        // is the same one used by the repository
        services
            .AddScoped<IAdminConnectionFactory,AdminConnectionFactory>()
            .AddScoped<ITenantConnectionFactory,TenantConnectionFactory>()
            .AddScoped<IWidgetRepository, WidgetRepository>()
            .AddScoped<ITenantStatisticsRepository, TenantStatisticsRepository>()
            .AddScoped<ITenantRepository, TenantRepository>();

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