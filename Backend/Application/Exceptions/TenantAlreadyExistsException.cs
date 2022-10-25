namespace Backend.Application.Exceptions;

internal class TenantAlreadyExistsException : BaseAlreadyExistsException
{
    public TenantAlreadyExistsException(Guid id) : base("tenant", id.ToString())
    {
    }
}