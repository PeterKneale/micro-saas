using Backend.Modules.Tenants.Domain.Common;
using Backend.Modules.Tenants.Domain.RegistrationAggregate;
using Backend.Modules.Tenants.Domain.TenantAggregate;
using Backend.Modules.Tenants.Messages;
using DotNetCore.CAP;

namespace Backend.Modules.Tenants.Application.IntegrationEvents.OnTenantClaimed;

internal class CreateTenant : ICapSubscribe
{
    private readonly IRegistrationRepository _registrations;
    private readonly ITenantRepository _tenants;
    private readonly ICapPublisher _publisher;

    public CreateTenant(IRegistrationRepository registrations, ITenantRepository tenants, ICapPublisher publisher)
    {
        _registrations = registrations;
        _tenants = tenants;
        _publisher = publisher;
    }

    [CapSubscribe("tenant-claimed", Group = "create-tenant")]
    public async Task ProcessAsync(TenantClaimedIntegrationEvent message, CancellationToken cancellationToken)
    {
        var registrationId = RegistrationId.CreateInstance(message.RegistrationId);

        var registration = await _registrations.Get(registrationId, cancellationToken);
        if (registration == null)
        {
            throw new RegistrationNotFoundException(registrationId.Id);
        }

        var tenantId = TenantId.CreateInstance();
        var name = registration.Name;
        var identifier = registration.Identifier;
        var tenant = Tenant.Provision(tenantId, name, identifier);
        await _tenants.Insert(tenant, cancellationToken);

        var integrationEvent = new TenantCreatedIntegrationEvent {TenantId = tenantId.Id};
        await _publisher.PublishAsync("tenant-created", integrationEvent, cancellationToken: cancellationToken);
    }
}