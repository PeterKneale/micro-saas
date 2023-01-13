using Backend.Modules.Registration.Messages;
using Backend.Modules.Tenants.Domain.TenantAggregate;

namespace Backend.Modules.Tenants.Application.IntegrationEvents.OnTenantClaimed;

internal class CreateTenant : ICapSubscribe
{
    private readonly ITenantRepository _tenants;
    private readonly ICapPublisher _publisher;

    public CreateTenant(ITenantRepository tenants, ICapPublisher publisher)
    {
        _tenants = tenants;
        _publisher = publisher;
    }

    [CapSubscribe(Topics.TenantClaimed)]
    public async Task ProcessAsync(TenantClaimedIntegrationEvent message, CancellationToken cancellationToken)
    {
        var tenantId = TenantId.CreateInstance();
        var name = TenantName.CreateInstance(message.Name);
        var identifier = TenantIdentifier.CreateInstance(message.Identifier);
        var tenant = Tenant.Provision(tenantId, name, identifier);
        await _tenants.Insert(tenant, cancellationToken);

        await _publisher.PublishAsync(Topics.TenantReady, new TenantReadyIntegrationEvent
        {
            RegistrationId = message.RegistrationId,
            TenantId = tenantId.Id
        }, cancellationToken: cancellationToken);
    }
}