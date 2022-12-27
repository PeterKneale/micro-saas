using System.Reflection;
using Backend.Modules.Statistics.Application.Contracts;
using Backend.Modules.Statistics.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Modules.Statistics;

public static class ServiceCollectionExtensions
{
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