using System.Reflection;
using Backend.Modules.Statistics.Api;
using Backend.Modules.Statistics.Application.Contracts;
using Backend.Modules.Statistics.Infrastructure;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Modules.Statistics;

public static class ServiceCollectionExtensions
{
    public static IEndpointRouteBuilder AddStatistics(this IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<StatisticsApi>();
        return builder;
    }
    
    public static IGrpcServerBuilder AddStatistics(this IGrpcServerBuilder builder)
    {
        return builder;
    }
    public static IServiceCollection AddStatistics(this IServiceCollection services, IConfiguration configuration)
    { 
        // application
        var assembly = Assembly.GetExecutingAssembly();
        services
            .AddMediatR(assembly)
            .AddValidatorsFromAssembly(assembly);
        
        services
            .AddScoped<ITenantStatisticsRepository, TenantStatisticsRepository>();
        
        return services;
    }
}