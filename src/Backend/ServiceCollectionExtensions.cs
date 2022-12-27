using Backend.Modules;
using Backend.Modules.Infrastructure.Interceptors;
using Backend.Modules.Settings;
using Backend.Modules.Settings.Api;
using Backend.Modules.Statistics;
using Backend.Modules.Tenants;
using Backend.Modules.Widgets;
using Backend.Modules.Widgets.Api;

namespace Backend;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
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
            })
            .AddServiceOptions<TenantSettingsApi>(options => {
                // Capture the tenant context from grpc metadata
                options.Interceptors.Add<TenantContextInterceptor>();   
            });

        services
            .AddModules(configuration)
            .AddTenants(configuration)
            .AddSettings(configuration)
            .AddStatistics(configuration)
            .AddWidgets(configuration);
        return services;
    }
}