﻿using Microsoft.Extensions.DependencyInjection;

namespace Modules.Tenants;
 
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTenantsModule(this IServiceCollection services)
    {
        return services.AddScoped<ITenantsModule, TenantsModule>();
    }
}