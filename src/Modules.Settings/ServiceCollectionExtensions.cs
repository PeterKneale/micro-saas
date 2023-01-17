namespace Modules.Settings;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSettingsModule(this IServiceCollection services)
    {
        return services.AddScoped<ISettingsModule, SettingsModule>();
    }
}