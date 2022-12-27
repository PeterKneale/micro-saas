using System.Diagnostics;
using System.Net;
using Backend.Modules;
using Backend.Modules.Infrastructure.Database;
using Backend.Modules.Infrastructure.Interceptors;
using Backend.Modules.Settings;
using Backend.Modules.Statistics;
using Backend.Modules.Tenants;
using Backend.Modules.Widgets;
using Microsoft.AspNetCore.Server.Kestrel.Core;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(opt =>
{
    // Operate one port in HTTP/1.1 mode for k8s health-checks etc
    opt.Listen(IPAddress.Any, 5000, listen => listen.Protocols = HttpProtocols.Http1);
    // Operate one port in HTTP/2 mode for GRPC
    opt.Listen(IPAddress.Any, 5001, listen => listen.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc(options =>
    {
        // Start trapping for exceptions and translate to grpc status codes
        options.Interceptors.Add<ExceptionInterceptor>();
        // Validate the grpc request
        options.Interceptors.Add<ValidationInterceptor>();
    })
    .AddSettings()
    .AddStatistics()
    .AddTenants()
    .AddWidgets();

builder.Services
    .AddModules(builder.Configuration)
    .AddSettings(builder.Configuration)
    .AddStatistics(builder.Configuration)
    .AddTenants(builder.Configuration)
    .AddWidgets(builder.Configuration);

builder.Services.AddHealthChecks();
builder.Services.AddLogging(c => { c.AddSimpleConsole(opt => { opt.SingleLine = true; }); });

var app = builder.Build();
app.MapGet("/", () => "Backend");
app.MapHealthChecks("/health/alive");
app.MapHealthChecks("/health/ready");
app
    .AddSettings()
    .AddStatistics()
    .AddTenants()
    .AddWidgets();
app.Services.ExecuteDatabaseMigration(x => x.ApplyDatabaseMigrations());
app.Run();