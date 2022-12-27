using Backend.Modules.Infrastructure.Interceptors;
using Backend.Modules.Widgets.Api;
using Backend.Modules.Widgets.Application.Contracts;
using Backend.Modules.Widgets.Infrastructure;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Backend.Modules.Widgets;

public static class ServiceCollectionExtensions
{
    public static IEndpointRouteBuilder AddWidgets(this IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<WidgetsApi>();
        return builder;
    }
    
    public static IGrpcServerBuilder AddWidgets(this IGrpcServerBuilder builder)
    {
        builder.AddServiceOptions<WidgetsApi>(options =>
        {
            // The widgets api requires tenant context
            options.Interceptors.Add<TenantContextInterceptor>();
        });
        return builder;
    }

    public static IServiceCollection AddWidgets(this IServiceCollection services, IConfiguration configuration)
    { 
        // application
        var assembly = Assembly.GetExecutingAssembly();
        services
            .AddMediatR(assembly)
            .AddValidatorsFromAssembly(assembly);
        
        // infrastructure
        services
            .AddScoped<IWidgetRepository, WidgetRepository>();
        
        return services;
    }
}