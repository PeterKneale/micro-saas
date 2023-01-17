namespace Modules.Infrastructure.Tenancy;

public interface IExecutionContextAccessor
{
    Guid CurrentTenant { get; }
}