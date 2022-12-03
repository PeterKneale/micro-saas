using Backend.Api;

namespace Frontend.Pages.Widgets;

public class Index : PageModel
{
    private readonly WidgetService.WidgetServiceClient _client;
    
    public Index(WidgetService.WidgetServiceClient client)
    {
        _client = client;
    }
    
    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var response = await _client.ListWidgetsAsync(new ListWidgetsRequest(), cancellationToken: cancellationToken);
        Items = response.Items.ToList();
    }
    
    public IEnumerable<ListWidgetsResponseItem> Items { get; private set; }
}