using System.Reflection;
using Backend.Modules.Application;
using Backend.Modules.Infrastructure.Behaviours;
using Backend.Modules.Infrastructure.Configuration;
using Backend.Modules.Infrastructure.Database;
using Backend.Modules.Infrastructure.Emails;
using Backend.Modules.Infrastructure.Tenancy;
using Backend.Modules.Tenants.Application.Contracts;
using Backend.Modules.Tenants.Infrastructure;
using FluentMigrator.Runner;
using Grpc.AspNetCore.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Modules.Tenants;

public static class ServiceCollectionExtensions
{
    public static IEndpointRouteBuilder AddTenants(this IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<Api.TenantsApi>();
        return builder;
    }
    
    public static IGrpcServerBuilder AddTenants(this IGrpcServerBuilder builder)
    {
        return builder;
    }
    
    public static IServiceCollection AddTenants(this IServiceCollection services, IConfiguration configuration)
    {
        
        // application
        var assembly = Assembly.GetExecutingAssembly();
        services
            .AddMediatR(assembly)
            .AddValidatorsFromAssembly(assembly);
        
        services
            .AddScoped<IRegistrationRepository, RegistrationRepository>()
            .AddScoped<ITenantRepository, TenantRepository>();

        return services;
    }
}