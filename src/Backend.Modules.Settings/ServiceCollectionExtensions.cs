using System.Reflection;
using Backend.Modules.Infrastructure.Interceptors;
using Backend.Modules.Settings.Infrastructure;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Modules.Settings;

public static class ServiceCollectionExtensions
{
    public static IEndpointRouteBuilder AddSettings(this IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<Backend.Modules.Settings.Api.SettingsApi>();
        return builder;
    }
    
    public static IGrpcServerBuilder AddSettings(this IGrpcServerBuilder builder)
    {
        builder.AddServiceOptions<Backend.Modules.Settings.Api.SettingsApi>(options =>
        {
            // The widgets api requires tenant context
            options.Interceptors.Add<TenantContextInterceptor>();
        });
        return builder;
    }
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        // application
        var assembly = Assembly.GetExecutingAssembly();
        services
            .AddMediatR(assembly)
            .AddValidatorsFromAssembly(assembly);
        
        services
            .AddScoped<ISettingsRepository, SettingsRepository>();
        
        return services;
    }
}