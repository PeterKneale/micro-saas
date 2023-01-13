namespace Backend.Modules.Registration.Messages;

public class TenantClaimedIntegrationEvent
{
    public Guid RegistrationId { get; init; }
    public string Name { get; set; }
    public string Identifier { get; set; }
}