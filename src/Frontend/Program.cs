using Finbuckle.MultiTenant;
using Frontend;
using Frontend.Infrastructure;
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
        var baseUri = builder.Configuration.GetServiceHttpUri();
        var readyUri = new Uri(baseUri, "/health/ready");
        using var client = new HttpClient();
        var response = await client.GetAsync(readyUri);
        return response.IsSuccessStatusCode
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy();
    }, tags: new[] {"ready"});


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

builder.Services.AddSingleton<CustomMultiTenantStore>();
builder.Services
    .AddMultiTenant<TenantInfo>()
    .WithStaticStrategy(builder.Configuration.GetTenant())
    .WithStore(ServiceLifetime.Singleton, sp => sp.GetRequiredService<CustomMultiTenantStore>());

builder.Services
    .AddGrpcClient<Backend.Api.TenantsApi.TenantsApiClient>(o => {
        o.Address = builder.Configuration.GetServiceGrpcUri();
    });

builder.Services
    .AddGrpcClient<Backend.Api.SettingsApi.SettingsApiClient>(o => {
        o.Address = builder.Configuration.GetServiceGrpcUri();
    }).AddInterceptor<TenantContextInterceptor>();;

builder.Services
    .AddGrpcClient<Backend.Api.WidgetsApi.WidgetsApiClient>(o => {
        o.Address = builder.Configuration.GetServiceGrpcUri();
    }).AddInterceptor<TenantContextInterceptor>();

builder.Services
    .AddScoped<TenantContextInterceptor>();

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
app.MapHealthChecks("/health/alive", new HealthCheckOptions
{
    Predicate = _ => false
}).AllowAnonymous();
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("ready")
}).AllowAnonymous();
app.UseMultiTenant();
app.Use(async (ctx, next) => {
    // redirect if no tenant context available
    if (ctx.GetMultiTenantContext<TenantInfo>()?.TenantInfo == null)
    {
        var redirectUri = app.Configuration.GetRegistrationHttpUri();
        ctx.Response.Redirect(redirectUri.ToString());
        return;
    }
    await next();
});
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();