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
    .AddAsyncCheck("Backend", async () =>
    {
        using var client = new HttpClient();
        var baseUri = builder.Configuration.GetServiceHttpUri();
        var readyUri = new Uri(baseUri, "/health/ready");
        var response = await client.GetAsync(readyUri);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy();
    }, tags: new[] {"ready"});

builder.Services
    .AddLogging(c => { c.AddSimpleConsole(opt => { opt.SingleLine = true; }); });

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services
    .AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

builder.Services.AddGrpcClient<Backend.Api.TenantsApi.TenantsApiClient>(o =>
{
    o.Address = builder.Configuration.GetServiceGrpcUri();
});
builder.Services.AddGrpcClient<Backend.Api.StatisticsApi.StatisticsApiClient>(o =>
{
    o.Address = builder.Configuration.GetServiceGrpcUri();
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.MapHealthChecks("/health/alive", new HealthCheckOptions
{
    Predicate = _ => false
}).AllowAnonymous();
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("ready")
}).AllowAnonymous();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();