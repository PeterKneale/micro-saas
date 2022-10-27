using Backend.Domain.WidgetAggregate;

namespace Backend.UnitTests.WidgetAggregate;

public class WidgetAggregateTests
{
    [Fact]
    public void WidgetsCanBeCreated()
    {
        Widget.CreateInstance(WidgetId.CreateInstance());
    }

    [Fact]
    public void WidgetCanBeUpdateed()
    {
        // arrange
        var widgetId = WidgetId.CreateInstance();
        var car = Widget.CreateInstance(widgetId);
        var registration = Description.CreateInstance("ACB-123");
        
        // act
        car.Update(registration);

        // assert
        car.Description.Should().BeEquivalentTo(registration);
    }
}