namespace Backend.Application.Exceptions;

internal class CarRegistrationUnavailableException : BaseRuleBrokenException
{
    public CarRegistrationUnavailableException(string registration) : base($"Registration {registration} already exists")
    {
    }
}