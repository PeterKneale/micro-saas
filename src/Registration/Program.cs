using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Registration;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services
    .AddHealthChecks()
    .AddAsyncCheck("Backend", async () => {
        var baseUri = builder.Configuration.GetServiceHttpUri("backend");
        var readyUri = new Uri(baseUri, "/health/ready");
        using var client = new HttpClient();
        var response = await client.GetAsync(readyUri);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy();
    }, tags: new[] { "ready" });


builder.Services
    .AddLogging(c => {
        c.AddSimpleConsole(opt => {
            opt.SingleLine = true;
        });
    });

builder.Services
    .AddGrpcClient<TenantsApi.TenantsApiClient>(o => {
        o.Address = builder.Configuration.GetServiceGrpcUri("backend");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapHealthChecks("/health/alive");
app.MapHealthChecks("/health/ready", new HealthCheckOptions {
    Predicate = r => r.Tags.Contains("ready")
});
app.UseAuthorization();
app.MapRazorPages();
app.Run();