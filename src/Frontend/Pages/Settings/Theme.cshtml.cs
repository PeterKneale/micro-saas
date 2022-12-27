using Backend.Api;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Pages.Settings;

public class Theme : PageModel
{
    private readonly TenantSettingsService.TenantSettingsServiceClient _client;

    public Theme(TenantSettingsService.TenantSettingsServiceClient client)
    {
        _client = client;
    }

    [BindProperty] public Model Data { get; set; }

    public async Task OnGetAsync()
    {
        Data = new Model
        {
            Themes = new SelectList(Themes, nameof(ThemeItem.Name), nameof(ThemeItem.Value))
        };
        //
        // var response = await _client.GetThemeAsync(new GetThemeRequest());
        // Data.Themes.SelectedValue = response.Theme;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var theme = Data.Themes.SelectedValue.ToString();
        await _client.SetThemeAsync(new SetThemeRequest
        {
            Theme = theme
        });

        return RedirectToPage("Index");
    }

    public record Model
    {
        public SelectList Themes { get; set; }
    }

    public static readonly IEnumerable<ThemeItem> Themes = new[]
    {
        new ThemeItem("A", "Theme A"),
        new ThemeItem("B", "Theme B"),
        new ThemeItem("C", "Theme C"),
    };

    public record ThemeItem(string Name, string Value);
}