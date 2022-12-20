using Backend.Features.Tenancy.Application.Commands;
using Backend.Features.Tenancy.Application.Queries;

namespace Backend.Features.Tenancy.Api;

public class TenantSettingsApi : TenantSettingsService.TenantSettingsServiceBase
{
    private readonly IMediator _mediator;

    public TenantSettingsApi(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GetThemeResponse> GetTheme(GetThemeRequest request, ServerCallContext context)
    {
        var theme = await _mediator.Send(new GetTheme.Query());
        return new GetThemeResponse
        {
            Theme = theme
        };
    }

    public override async Task<EmptyResponse4> SetTheme(SetThemeRequest request, ServerCallContext context)
    {
        await _mediator.Send(new SetTheme.Command(request.Theme));
        return new EmptyResponse4();
    }

    public override async Task<EmptyResponse4> ResetTheme(ResetThemeRequest request, ServerCallContext context)
    {
        await _mediator.Send(new ResetTheme.Command());
        return new EmptyResponse4();
    }
}