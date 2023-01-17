using System.Diagnostics;
using System.Net;
using System.Reflection;
using Backend.Api;
using Backend.Infrastructure.ExecutionContext;
using Backend.Infrastructure.Interceptors;
using Backend.Modules;
using Backend.Modules.Infrastructure.Configuration;
using Backend.Modules.Registrations;
using Backend.Modules.Settings;
using Backend.Modules.Statistics;
using Backend.Modules.Tenants;
using Backend.Modules.Widgets;
using Microsoft.AspNetCore.Server.Kestrel.Core;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

var assembly = Assembly.GetExecutingAssembly();

var builder = WebApplication.CreateBuilder(args);

var logging = LoggerFactory.Create(c => { c.AddSimpleConsole(opt => { opt.SingleLine = true; }); });

builder.WebHost.ConfigureKestrel(opt =>
{
    // Operate one port in HTTP/1.1 mode for k8s health-checks etc
    opt.Listen(IPAddress.Any, 5000, listen => listen.Protocols = HttpProtocols.Http1);
    // Operate one port in HTTP/2 mode for GRPC
    opt.Listen(IPAddress.Any, 5001, listen => listen.Protocols = HttpProtocols.Http2);
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();
builder.Services.AddLogging(c => { c.AddSimpleConsole(opt => { opt.SingleLine = true; }); });
builder.Services.AddGrpc(options =>
{
    // Start trapping for exceptions and translate to grpc status codes
    options.Interceptors.Add<ExceptionInterceptor>();
    // Validate the grpc request
    options.Interceptors.Add<ValidationInterceptor>();
});

builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCapProcessing(builder.Configuration);
builder.Services
    .AddRegistrationsModule()
    .AddStatisticsModule()
    .AddTenantsModule()
    .AddSettingsModule()
    .AddWidgetsModule();

var app = builder.Build();

app.MapGet("/", () => "Backend");
app.MapHealthChecks("/health/alive");
app.MapHealthChecks("/health/ready");

app.MapGrpcService<RegistrationsApiService>();
app.MapGrpcService<SettingsApiService>();
app.MapGrpcService<StatisticsApiService>();
app.MapGrpcService<TenantsApiService>();
app.MapGrpcService<WidgetsApiService>();

var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
var executionContextAccessor = new ExecutionContextAccessorAccessor(httpContextAccessor);

RegistrationsModuleSetup.Init(executionContextAccessor, logging, builder.Configuration);
SettingsModuleSetup.Init(executionContextAccessor, logging, builder.Configuration);
StatisticsModuleSetup.Init(executionContextAccessor, logging, builder.Configuration);
TenantsModuleSetup.Init(executionContextAccessor, logging, builder.Configuration);
WidgetsModuleSetup.Init(executionContextAccessor, logging, builder.Configuration);

Backend.Modules.Registrations.Infrastructure.Database.MigrationRunner.Run(builder.Configuration.GetSystemConnectionString(), x => x.ResetDatabase());

WidgetsModuleSetup.SetupDatabase(x => x.ApplyDatabaseMigrations());
SettingsModuleSetup.SetupDatabase(x => x.ApplyDatabaseMigrations());
StatisticsModuleSetup.SetupDatabase(x => x.ApplyDatabaseMigrations());
TenantsModuleSetup.SetupDatabase(x => x.ApplyDatabaseMigrations());

TenantsModuleSetup.SetupOutbox();
RegistrationsModuleSetup.SetupOutbox();

app.Run();