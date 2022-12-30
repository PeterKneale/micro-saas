namespace Backend.Modules.Settings.Domain.SettingsAggregate;

public class Settings
{
    private Settings()
    {
        // Parameterless constructor for serialisation
    }

    public void SetTheme(string theme)
    {
        Theme = theme;
    }

    public void ResetTheme()
    {
        Theme = null;
    }

    public string? Theme { get; private set; }

    public string GetTheme() => Theme ?? DefaultThemeName;

    public static Settings Create() =>
        new();

    public const string DefaultThemeName = "Default";
}