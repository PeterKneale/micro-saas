using System.Diagnostics;
using System.Net;
using Backend;
using Backend.Core.Infrastructure.Database;
using Backend.Features.Tenancy.Api;
using Backend.Features.Widgets.Api;
using Microsoft.AspNetCore.Server.Kestrel.Core;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(opt => {
    // Operate one port in HTTP/1.1 mode for k8s health-checks etc
    opt.Listen(IPAddress.Any, 5000, listen => listen.Protocols = HttpProtocols.Http1);
    // Operate one port in HTTP/2 mode for GRPC
    opt.Listen(IPAddress.Any, 5001, listen => listen.Protocols = HttpProtocols.Http2);
});

builder.Services.AddBackend(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddLogging(c => {
    c.AddSimpleConsole(opt => {
        opt.SingleLine = true;
    });
});

var app = builder.Build();
app.MapGrpcService<TenantManagementApi>();
app.MapGrpcService<TenantStatisticsApi>();
app.MapGrpcService<WidgetApi>();
app.MapGet("/", () => "Backend");
app.MapHealthChecks("/health/alive");
app.MapHealthChecks("/health/ready");
app.Services.ExecuteDatabaseMigration(x => x.ApplyDatabaseMigrations());
app.Run();