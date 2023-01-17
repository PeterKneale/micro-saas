using Microsoft.Extensions.DependencyInjection;

namespace Modules.Registrations;
 
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRegistrationsModule(this IServiceCollection services)
    {
        return services.AddScoped<IRegistrationsModule, RegistrationsModule>();
    }
}