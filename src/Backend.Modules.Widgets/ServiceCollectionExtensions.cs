namespace Backend.Modules.Widgets;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWidgetsModule(this IServiceCollection services)
    {
        return services
            .AddScoped<IWidgetsModule, WidgetsModule>();
    }
}