using Backend.Modules.Widgets.Domain.WidgetAggregate;

namespace Backend.Modules.Widgets.UnitTests.Domain;

public class WidgetAggregateTests
{
    [Fact]
    public void WidgetsCanBeCreated()
    {
        Widget.CreateInstance(WidgetId.CreateInstance(), Description.CreateInstance("x"));
    }

    [Fact]
    public void WidgetCanBeUpdated()
    {
        // arrange
        var widgetId = WidgetId.CreateInstance();
        var widget = Widget.CreateInstance(widgetId, Description.CreateInstance("x"));
        var description = Description.CreateInstance("ACB-123");
        
        // act
        widget.Update(description);

        // assert
        widget.Description.Should().BeEquivalentTo(description);
    }
}