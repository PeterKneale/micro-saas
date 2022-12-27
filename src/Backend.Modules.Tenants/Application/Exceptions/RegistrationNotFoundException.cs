namespace Backend.Modules.Tenants.Application.Exceptions;

internal class RegistrationNotFoundException : BaseNotFoundException
{
    public RegistrationNotFoundException(string identifier) : base("registration", identifier)
    {
    }
    public RegistrationNotFoundException(Guid id) : base("registration", id.ToString())
    {
    }
}