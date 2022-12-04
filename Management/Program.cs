using System.Diagnostics;
using Management;
using Microsoft.AspNetCore.Authorization;

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

builder.Services
    .AddGrpcClient<Backend.Api.TenantManagementService.TenantManagementServiceClient>(o => {
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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();