namespace Backend.Modules.Tenants.Messages;

public class TenantReadyIntegrationEvent
{
    public Guid RegistrationId { get; set; }
    public Guid TenantId { get; set; }
}