using Microsoft.Extensions.DependencyInjection;

namespace Modules.Statistics;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStatisticsModule(this IServiceCollection services)
    { 
        return services.AddScoped<IStatisticsModule, StatisticsModule>();
    }
}