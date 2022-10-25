namespace Backend.Application.Exceptions;


internal class CarAlreadyRegisteredException : BaseRuleBrokenException
{
    public CarAlreadyRegisteredException(string registration) : base($"Car {registration} already registered")
    {
    }
}