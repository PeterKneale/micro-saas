using Backend.Api;
using Backend.Modules.Widgets.Application.Commands;
using Backend.Modules.Widgets.Application.Queries;

namespace Backend.Modules.Widgets.Api;

public class WidgetsApi : Backend.Api.WidgetsApi.WidgetsApiBase
{
    private readonly IMediator _mediator;

    public WidgetsApi(IMediator mediator)
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