﻿using Backend.Api;

namespace Frontend.Pages.Widgets;

public class Add : PageModel
{
    private readonly WidgetService.WidgetServiceClient _client;

    public Add(WidgetService.WidgetServiceClient client)
    {
        _client = client;
    }

    [BindProperty]
    public Model Data { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        await _client.AddWidgetAsync(new AddWidgetRequest
        {
            Id = Guid.NewGuid().ToString(),
            Description = Data.Description
        });

        return RedirectToPage("Index");
    }

    public record Model
    {
        public string Description { get; init; }
    }
}