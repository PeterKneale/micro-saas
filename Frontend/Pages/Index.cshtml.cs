using Finbuckle.MultiTenant;

namespace Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }
    
    public TenantInfo TenantInfo { get; private set; }

    public void OnGet()
    {
        TenantInfo = HttpContext
            .GetMultiTenantContext<TenantInfo>()?
            .TenantInfo!;
    }
}