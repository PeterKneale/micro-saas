using Modules.Infrastructure.Tenancy;

namespace Modules.Settings.IntegrationTests.Fixtures;

internal class FakeExecutionContextAccessor : IExecutionContextAccessor
{
    private Guid _value;

    public void SetCurrentTenant(Guid value)
    {
        _value = value;
    }

    public Guid CurrentTenant => _value;
}