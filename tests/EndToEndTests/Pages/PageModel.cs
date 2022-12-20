namespace EndToEndTests.Pages;

public abstract class PageModel
{
    protected readonly IPage Page;

    protected PageModel(IPage page)
    {
        Page = page;
    }
    
    public Task<string> GetTitle => 
        Page.TitleAsync();
}