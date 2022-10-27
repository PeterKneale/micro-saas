using Backend.Domain.WidgetAggregate;

namespace Backend.UnitTests;

public class SmokeTests
{
    [Fact]
    public void SmokeTest()
    {
        var widgetId = WidgetId.CreateInstance();
        var car = Widget.CreateInstance(widgetId);
        var registration = Description.CreateInstance("ACB-123");
        car.Update(registration);
    }
}