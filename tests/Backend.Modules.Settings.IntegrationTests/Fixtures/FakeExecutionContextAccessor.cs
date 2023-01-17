using Backend.Modules.Infrastructure.Tenancy;

namespace Backend.Modules.Settings.IntegrationTests.Fixtures;

internal class FakeExecutionContextAccessor : IExecutionContextAccessor
{
    private Guid _value;

    public void SetCurrentTenant(Guid value)
    {
        _value = value;
    }

    public Guid CurrentTenant => _value;
}