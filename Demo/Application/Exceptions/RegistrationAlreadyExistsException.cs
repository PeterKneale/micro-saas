namespace Demo.Application.Exceptions;

public class RegistrationAlreadyExistsException : Exception
{
    public RegistrationAlreadyExistsException(string registration) : base($"Registration {registration} already exists")
    {
    }
}