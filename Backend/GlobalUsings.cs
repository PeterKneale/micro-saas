// Global using directives
global using System.Data;
global using System.Reflection;
global using Backend.Api;
global using Backend.Core.Infrastructure.Repositories;
global using Backend.Core.Infrastructure.Repositories.Serialisation;
global using Backend.Features.Tenancy.Application.Contracts;
global using Backend.Features.Tenancy.Application.Exceptions;
global using Backend.Features.Tenancy.Application.Notifications;
global using Backend.Features.Tenancy.Domain.Common;
global using Dapper;
global using FluentMigrator.Runner;
global using FluentValidation;
global using Grpc.Core;
global using MediatR;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Npgsql;