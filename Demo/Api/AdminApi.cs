using Demo.Application.Queries;
using Grpc.Core;

namespace Demo.Api;

public class AdminApi : AdminService.AdminServiceBase
{
    private readonly IMediator _mediator;

    public AdminApi(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<CountCarsResponse> CountCars(CountCarsRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new CountCars.Query());
        return new CountCarsResponse {Total = result.Total};
    }
}