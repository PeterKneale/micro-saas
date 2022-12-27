using Backend.Modules.Widgets.Application.Contracts;
using Backend.Modules.Widgets.Infrastructure;

namespace Backend.Modules.Widgets;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWidgets(this IServiceCollection services, IConfiguration configuration)
    { 
        // api
        
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