using System.Diagnostics;
using System.Net;
using Backend;
using Backend.Modules.Infrastructure.Database;
using Backend.Modules.Settings.Api;
using Backend.Modules.Statistics.Api;
using Backend.Modules.Tenants.Api;
using Backend.Modules.Widgets.Api;
using Microsoft.AspNetCore.Server.Kestrel.Core;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;
Console.WriteLine("Starting");

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(opt => {
    // Operate one port in HTTP/1.1 mode for k8s health-checks etc
    opt.Listen(IPAddress.Any, 5000, listen => listen.Protocols = HttpProtocols.Http1);
    // Operate one port in HTTP/2 mode for GRPC
    opt.Listen(IPAddress.Any, 5001, listen => listen.Protocols = HttpProtocols.Http2);
});

builder.Services.AddServices(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddLogging(c => {
    c.AddSimpleConsole(opt => {
        opt.SingleLine = true;
    });
});

var app = builder.Build();
app.MapGrpcService<TenantAdminApi>();
app.MapGrpcService<TenantSettingsApi>();
app.MapGrpcService<TenantStatisticsApi>();
app.MapGrpcService<WidgetApi>();
app.MapGet("/", () => "Backend");
app.MapHealthChecks("/health/alive");
app.MapHealthChecks("/health/ready");
app.Services.ExecuteDatabaseMigration(x => x.ApplyDatabaseMigrations());
Console.WriteLine("Started");
app.Run();