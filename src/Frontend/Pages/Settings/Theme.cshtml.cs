using Backend.Api;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Pages.Settings;

public class Theme : PageModel
{
    private readonly SettingsApi.SettingsApiClient _client;

    public Theme(SettingsApi.SettingsApiClient client)
    {
        _client = client;
    }

    [BindProperty] public Model Data { get; set; }

    public async Task OnGetAsync()
    {
        var currentTheme = await GetCurrentTheme();
        Data = new Model
        {
            Themes = new SelectList(Themes, SelectListValueField, SelectListTextField, currentTheme)
        };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var theme = Data.Theme;
        await _client.SetThemeAsync(new SetThemeRequest
        {
            Theme = theme
        });

        return RedirectToPage(nameof(Theme));
    }

    private async Task<string> GetCurrentTheme()
    {
        var response = await _client.GetThemeAsync(new GetThemeRequest());
        return response.Theme;
    }

    public record Model
    {
        public string Theme { get; set; }
        public SelectList Themes { get; set; }
    }

    public static readonly IEnumerable<ThemeItem> Themes = new[]
    {
        new ThemeItem("Theme A", "A"),
        new ThemeItem("Theme B", "B"),
        new ThemeItem("Theme C", "C"),
        new ThemeItem("Theme D", "D"),
        new ThemeItem("Theme E", "E"),
    };

    public record ThemeItem(string Name, string Value);

    private const string SelectListTextField = nameof(ThemeItem.Name);
    private const string SelectListValueField = nameof(ThemeItem.Value);
}