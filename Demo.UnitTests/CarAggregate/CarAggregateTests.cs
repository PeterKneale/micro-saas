using Demo.Domain.CarAggregate;

namespace Demo.UnitTests.CarAggregate;

public class CarAggregateTests
{
    [Fact]
    public void CarsCanBeCreated()
    {
        Car.CreateInstance(CarId.CreateInstance());
    }

    [Fact]
    public void CarCanBeRegistered()
    {
        // arrange
        var carId = CarId.CreateInstance();
        var car = Car.CreateInstance(carId);
        var registration = Registration.CreateInstance("ACB-123");
        
        // act
        car.Register(registration);

        // assert
        car.Registration.Should().BeEquivalentTo(registration);
    }
}