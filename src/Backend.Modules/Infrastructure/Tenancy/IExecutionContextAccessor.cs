namespace Backend.Modules.Infrastructure.Tenancy;

public interface IExecutionContextAccessor
{
    Guid CurrentTenant { get; }
}