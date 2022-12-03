using Backend.Api;
using Backend.Features.Widgets.Application.Commands;
using Backend.Features.Widgets.Application.Queries;
using Grpc.Core;

namespace Backend.Features.Widgets.Api;

public class WidgetApi : WidgetService.WidgetServiceBase
{
    private readonly IMediator _mediator;

    public WidgetApi(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<EmptyResponse> AddWidget(AddWidgetRequest request, ServerCallContext context)
    {
        await _mediator.Send(new AddWidget.Command(Guid.Parse(request.Id), request.Description));
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
    public override async Task<ListWidgetsResponse> ListWidgets(ListWidgetsRequest request, ServerCallContext context)
    {
        var results = await _mediator.Send(new ListWidgets.Query());

        return new ListWidgetsResponse
        {
            Items =
            {
                results.Select(x => new ListWidgetsResponseItem
                {
                    Id = x.Id.ToString(),
                    Description = x.Description ?? string.Empty
                }).ToList()
            }
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
    
    public class UpdateWidgetValidator : AbstractValidator<GetWidgetRequest>
    {
        public UpdateWidgetValidator()
        {
            RuleFor(x => x.Id).NotNull()
                .NotEmpty()
                .Must(x => Guid.TryParse(x, out _))
                .WithMessage("'Id' must be a valid GUID");
        }
    }
}