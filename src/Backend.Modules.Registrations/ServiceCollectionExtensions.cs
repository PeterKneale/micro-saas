using Microsoft.Extensions.DependencyInjection;

namespace Backend.Modules.Registrations;
 
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRegistrationsModule(this IServiceCollection services)
    {
        return services.AddScoped<IRegistrationsModule, RegistrationsModule>();
    }
}