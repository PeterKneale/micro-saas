using Backend.FunctionalTests.Fixtures;

namespace Backend.FunctionalTests.UseCase.Tenancy;

[Collection(nameof(ServiceCollectionFixture))]
public class TenantThemeTests
{
    private readonly TenantSettingsService.TenantSettingsServiceClient _settings;
    private readonly TenantTestHelper _helper;
    private string DefaultThemeName = "Default";
    private string AlternateThemeName = "X";

    public TenantThemeTests(ServiceFixture service, ITestOutputHelper output)
    {
        service.OutputHelper = output;
        _settings = service.TenantSettingsClient;
        _helper = new TenantTestHelper(service.TenantAdminClient);
    }

    [Fact]
    public async Task Can_get_default_theme()
    {
        // arrange
        var metadata = await _helper.CreateTenant();

        // assert
        (await _settings.GetThemeAsync(new GetThemeRequest(), metadata))
            .Theme.Should().Be(DefaultThemeName);
    }
    
    [Fact]
    public async Task Can_set_theme()
    {
        // arrange
        var metadata = await _helper.CreateTenant();

        // act
        await _settings.SetThemeAsync(new SetThemeRequest {Theme = AlternateThemeName}, metadata);

        (await _settings.GetThemeAsync(new GetThemeRequest(), metadata))
            .Theme.Should().Be(AlternateThemeName);
    }

    [Fact]
    public async Task Can_reset_theme()
    {
        // arrange
        var metadata = await _helper.CreateTenant();

        // act
        await _settings.ResetThemeAsync(new ResetThemeRequest(), metadata);

        (await _settings.GetThemeAsync(new GetThemeRequest(), metadata))
            .Theme.Should().Be(DefaultThemeName);
    }

    [Fact]
    public async Task Can_get_set_get_reset_get_theme()
    {
        // arrange
        var metadata = await _helper.CreateTenant();

        // act
        (await _settings.GetThemeAsync(new GetThemeRequest(), metadata))
            .Theme.Should().Be(DefaultThemeName);

        await _settings.SetThemeAsync(new SetThemeRequest {Theme = AlternateThemeName}, metadata);

        (await _settings.GetThemeAsync(new GetThemeRequest(), metadata))
            .Theme.Should().Be(AlternateThemeName);

        await _settings.ResetThemeAsync(new ResetThemeRequest(), metadata);

        (await _settings.GetThemeAsync(new GetThemeRequest(), metadata))
            .Theme.Should().Be(DefaultThemeName);
    }
}