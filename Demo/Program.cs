using Demo;
using Demo.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDemo(builder.Configuration);
var app = builder.Build();

app.MapGrpcService<AdminApi>();
app.MapGrpcService<TenantApi>();
app.MapGet("/", () => "Demo");
app.ExecuteDatabaseMigration(x=>x.ApplyDatabaseMigrations());
app.Run();