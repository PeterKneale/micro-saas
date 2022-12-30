namespace Backend.Modules.Tenants.Messages;

public class TenantClaimedIntegrationEvent
{
    public Guid RegistrationId { get; set; }
}

public class TenantRegisteredIntegrationEvent
{
    public Guid RegistrationId { get; set; }
}