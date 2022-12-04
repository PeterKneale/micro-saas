using Finbuckle.MultiTenant;

namespace Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly IMultiTenantContextAccessor<TenantInfo> _accessor;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IMultiTenantContextAccessor<TenantInfo> accessor, ILogger<IndexModel> logger)
    {
        _accessor = accessor;
        _logger = logger;
    }
    
    public TenantInfo TenantInfo { get; private set; }

    public void OnGet()
    {
        TenantInfo = _accessor.MultiTenantContext?.TenantInfo;
    }
}