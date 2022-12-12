using Backend.Api;
using Backend.Features.Tenancy.Application.Commands;
using Grpc.Core;

namespace Backend.Features.Tenancy.Api;

public class TenantRegistrationApi : TenantRegistrationService.TenantRegistrationServiceBase
{
    private readonly IMediator _mediator;

    public TenantRegistrationApi(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<EmptyResponse3> Register(RegisterRequest request, ServerCallContext context)
    {
        await _mediator.Send(new RegisterTenant.Command(request.Email, request.Name, request.Identifier, request.OverrideToken));
        return new EmptyResponse3();
    }

    public override async Task<EmptyResponse3> Claim(ClaimRequest request, ServerCallContext context)
    {
        await _mediator.Send(new ClaimTenant.Command(request.Identifier, request.Password, request.Token));
        return new EmptyResponse3();
    }
}