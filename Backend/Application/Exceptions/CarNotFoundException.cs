namespace Backend.Application.Exceptions;

internal class CarBaseNotFoundException : BaseNotFoundException
{
    public CarBaseNotFoundException(Guid id) : base("car", id.ToString())
    {
    }
    public CarBaseNotFoundException(string registration) : base("car", registration)
    {
    }
}