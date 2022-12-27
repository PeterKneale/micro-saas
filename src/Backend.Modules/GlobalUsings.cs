// Global using directives

global using System.Data;
global using Backend.Modules.Application;
global using Backend.Modules.Infrastructure.Configuration;
global using Backend.Modules.Infrastructure.Repositories;
global using Backend.Modules.Infrastructure.Tenancy;
global using Dapper;
global using FluentValidation;
global using MediatR;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Npgsql;
global using Polly;
global using Polly.Retry;