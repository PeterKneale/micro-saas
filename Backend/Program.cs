using Backend;
using Backend.Api;
using Backend.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBackend(builder.Configuration);
builder.Services.AddLogging(c => {
    c.AddSimpleConsole(opt => {
        opt.SingleLine = true;
    });
});
var app = builder.Build();

app.MapGrpcService<AdminApi>();
app.MapGrpcService<TenantApi>();
app.MapGet("/", () => "Backend");
app.ExecuteDatabaseMigration(x=>x.ApplyDatabaseMigrations());
app.Run();