using Demo.Domain.CarAggregate;

namespace Demo.UnitTests;

public class SmokeTests
{
    [Fact]
    public void SmokeTest()
    {
        var carId = CarId.CreateInstance();
        var car = Car.CreateInstance(carId);
        var registration = Registration.CreateInstance("ACB-123");
        car.Register(registration);
    }
}