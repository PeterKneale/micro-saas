using System.Diagnostics;
using Finbuckle.MultiTenant;
using Frontend;
using Frontend.Infrastructure;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorPages()
    .AddRazorRuntimeCompilation();

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
    .AddGrpcClient<Backend.Api.TenantManagementService.TenantManagementServiceClient>(o => {
        o.Address = builder.Configuration.GetServiceGrpcUri("backend");
    });

builder.Services
    .AddGrpcClient<Backend.Api.WidgetService.WidgetServiceClient>(o => {
        o.Address = builder.Configuration.GetServiceGrpcUri("backend");
    }).AddInterceptor<TenantInterceptor>();

builder.Services
    .AddScoped<TenantInterceptor>();

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
app.UseMultiTenant();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();