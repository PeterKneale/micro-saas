using System.Reflection;
using Backend.Modules.Settings.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Modules.Settings;

public static class ServiceCollectionExtensions
{
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