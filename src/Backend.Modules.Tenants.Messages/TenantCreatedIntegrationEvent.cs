namespace Backend.Modules.Tenants.Messages;

public class TenantCreatedIntegrationEvent
{
    public Guid TenantId { get; set; }
}