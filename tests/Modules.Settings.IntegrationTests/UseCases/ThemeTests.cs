using Modules.Settings.Application.Commands;
using Modules.Settings.Application.Queries;
using Modules.Settings.IntegrationTests.Fixtures;

namespace Modules.Settings.IntegrationTests.UseCases;

[Collection(nameof(ContainerCollectionFixture))]
public class ThemeTests
{
    private readonly IServiceProvider _provider;

    public ThemeTests(ContainerFixture container)
    {
        _provider = container.Provider;
    }

    [Fact]
    public async Task Can_get_initial_default_theme()
    {
        // arrange
        var id = Guid.NewGuid();

        // act
        await AssertThemeIsDefault(id);
    }

    [Fact]
    public async Task Can_change_themes()
    {
        // arrange
        var id = Guid.NewGuid();
        var theme1 = Guid.NewGuid().ToString();        
        var theme2 = Guid.NewGuid().ToString();       
        var theme3 = Guid.NewGuid().ToString();

        // act && assert
        await _provider.ExecuteCommand(new SetTheme.Command(theme1), id);
        await AssertThemeIs(id, theme1);

        await _provider.ExecuteCommand(new SetTheme.Command(theme2), id);
        await AssertThemeIs(id, theme2);
        
        await _provider.ExecuteCommand(new SetTheme.Command(theme3), id);
        await AssertThemeIs(id, theme3);
    }

    private async Task AssertThemeIs(Guid id, string theme1)
    {
        var result1 = await _provider.ExecuteQuery(new GetTheme.Query(), id);
        result1.Should().Be(theme1);
    }

    [Fact]
    public async Task Can_reset_themes()
    {
        // arrange
        var id = Guid.NewGuid();
        var theme = Guid.NewGuid().ToString();        

        // act
        await _provider.ExecuteCommand(new SetTheme.Command(theme), id);
        await _provider.ExecuteCommand(new ResetTheme.Command(), id);

        // assert
        await AssertThemeIsDefault(id);
    }

    private async Task AssertThemeIsDefault(Guid id)
    {
        var result = await _provider.ExecuteQuery(new GetTheme.Query(), id);
        result.Should().Be(Domain.SettingsAggregate.Settings.DefaultThemeName);
    }
}