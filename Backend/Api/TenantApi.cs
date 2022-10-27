using Backend.Application.Commands;
using Backend.Application.Commands.Tenants;
using Backend.Application.Queries;
using Backend.Application.Queries.Tenants;
using Grpc.Core;

namespace Backend.Api;

public class TenantApi : TenantService.TenantServiceBase
{
    private readonly IMediator _mediator;

    public TenantApi(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<EmptyResponse> AddWidget(AddWidgetRequest request, ServerCallContext context)
    {
        await _mediator.Send(new AddWidget.Command(Guid.Parse(request.Id)));
        return new EmptyResponse();
    }
    
    public override async Task<EmptyResponse> UpdateWidget(UpdateWidgetRequest request, ServerCallContext context)
    {
        await _mediator.Send(new UpdateWidget.Command(Guid.Parse(request.Id), request.Description));
        return new EmptyResponse();
    }

    public override async Task<GetWidgetResponse> GetWidget(GetWidgetRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetWidget.Query(Guid.Parse(request.Id)));
        return new GetWidgetResponse
        {
            Id = result.Id.ToString(),
            Description = result.Description ?? string.Empty
        };
    }

    public class GetWidgetValidator : AbstractValidator<GetWidgetRequest>
    {
        public GetWidgetValidator()
        {
            RuleFor(x => x.Id).NotNull()
                .NotEmpty()
                .Must(x => Guid.TryParse(x, out _))
                .WithMessage("'Id' must be a valid GUID");
        }
    }
}