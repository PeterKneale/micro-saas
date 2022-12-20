using Backend.Features.Tenancy.Application.Commands;
using Backend.Features.Tenancy.Application.Queries;

namespace Backend.Features.Tenancy.Api;

public class TenantAdminApi : TenantAdminService.TenantAdminServiceBase
{
    private readonly IMediator _mediator;

    public TenantAdminApi(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async Task<GetTenantResponse> GetTenant(GetTenantRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetTenant.Query(Guid.Parse(request.Id)));
        return new GetTenantResponse
        {
            Id = result.Id.ToString(),
            Name = result.Name,
            Identifier = result.Identifier
        };
    }
    
    public override async Task<GetTenantByIdentifierResponse> GetTenantByIdentifier(GetTenantByIdentifierRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetTenantByIdentifier.Query(request.Identifier));
        return new GetTenantByIdentifierResponse
        {
            Id = result.Id.ToString(),
            Name = result.Name,
            Identifier = result.Identifier
        };
    }
    
    public override async Task<EmptyResponse2> AddTenant(AddTenantRequest request, ServerCallContext context)
    {
        await _mediator.Send(new AddTenant.Command(Guid.Parse(request.Id), request.Name, request.Identifier));
        return new EmptyResponse2();
    }
    
    public override async Task<ListTenantsResponse> ListTenants(ListTenantsRequest request, ServerCallContext context)
    {
        var results = await _mediator.Send(new ListTenants.Query());
        var items = results.Select(x => new ListTenantsItem
        {
            Id = x.Id.ToString(),
            Name = x.Name,
            Identifier = x.Identifier
        });
        return new ListTenantsResponse
        {
            Items = {items}
        };
    }

    public override async Task<EmptyResponse2> Register(RegisterRequest request, ServerCallContext context)
    {
        await _mediator.Send(new RegisterTenant.Command(request.Email, request.Name, request.Identifier, request.OverrideToken));
        return new EmptyResponse2();
    }

    public override async Task<EmptyResponse2> Claim(ClaimRequest request, ServerCallContext context)
    {
        await _mediator.Send(new ClaimTenant.Command(request.Identifier, request.Password, request.Token));
        return new EmptyResponse2();
    }
}