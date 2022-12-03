using Backend.Application.Commands.Admin;
using Backend.Application.Queries;
using Backend.Application.Queries.Admin;
using Grpc.Core;

namespace Backend.Api;

public class AdminApi : AdminService.AdminServiceBase
{
    private readonly IMediator _mediator;

    public AdminApi(IMediator mediator)
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

    public override async Task<GetDashboardResponse> GetDashboard(GetDashboardRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetDashboard.Query());
        return new GetDashboardResponse
        {
            TotalTenants = result.TotalTenants,
            TotalWidgets = result.TotalWidgets
        };
    }
}