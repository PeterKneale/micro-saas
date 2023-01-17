using System.Reflection;
using Backend.Modules.Infrastructure.Configuration;
using Backend.Modules.Infrastructure.Database;
using Backend.Modules.Infrastructure.Tenancy;
using Backend.Modules.Registrations.Application.Contracts;
using Backend.Modules.Registrations.Infrastructure.Database;
using Backend.Modules.Registrations.Infrastructure.Repositories;
using DotNetCore.CAP.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Modules.Registrations;

public static class RegistrationsModuleSetup
{
    private static ServiceProvider? _provider;

    public static void Init(IExecutionContextAccessor context, ILoggerFactory logger, IConfiguration configuration, Action<IServiceCollection>? overrides=null)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        var services = new ServiceCollection();

        // passed from host 
        services.AddLogging();
        services.AddSingleton(context);
        services.AddSingleton(logger);
        services.AddSingleton(configuration);
        
        // add core 
        services.AddModules(configuration);
            
        // application
        services.AddMediatR(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services
            .AddScoped<Application.IntegrationEvents.OnTenantRegistered.SendEmail>()
            .AddScoped<Application.IntegrationEvents.OnTenantReady.SendEmail>();

        // infrastructure
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
        
        _provider = services.BuildServiceProvider();

        RegistrationsCompositionRoot.SetProvider(_provider);
    }
    
    public static void SetupDatabase(Action<MigrationExecutor> action)
    {
        var configuration = _provider.GetRequiredService<IConfiguration>();
        var connection = configuration.GetSystemConnectionString();
        MigrationRunner.Run(connection, action);
    }

    public static void SetupOutbox()
    {
        _provider.GetRequiredService<IBootstrapper>()
            .BootstrapAsync()
            .GetAwaiter()
            .GetResult();
    }
}