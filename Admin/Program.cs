using System.Diagnostics;
using Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

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
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services
    .AddAuthorization(options => {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

builder.Services
    .AddGrpcClient<Backend.Api.TenantAdminService.TenantAdminServiceClient>(o => {
        o.Address = builder.Configuration.GetServiceGrpcUri("backend");
    });
builder.Services
    .AddGrpcClient<Backend.Api.TenantStatisticsService.TenantStatisticsServiceClient>(o => {
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
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();