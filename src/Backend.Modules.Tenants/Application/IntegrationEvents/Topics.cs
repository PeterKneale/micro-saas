namespace Backend.Modules.Tenants.Application.IntegrationEvents;

public static class Topics
{
    public const string TenantRegistered = "tenant-registered";
    public const string TenantClaimed = "tenant-claimed";
    public const string TenantReady = "tenant-ready";
}