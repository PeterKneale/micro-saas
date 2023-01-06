using Backend.Modules.Widgets;
using Backend.Modules.Widgets.Application.Commands;
using Backend.Modules.Widgets.Application.Queries;
using FluentValidation;
using Grpc.Core;

namespace Backend.Api;

public class WidgetsApiService : WidgetsApi.WidgetsApiBase
{
    private readonly IWidgetsModule _module;

    public WidgetsApiService(IWidgetsModule module)
    {
        _module = module;
    }

    public override async Task<EmptyResponse> AddWidget(AddWidgetRequest request, ServerCallContext context)
    {
        await _module.ExecuteCommandAsync(new AddWidget.Command(Guid.Parse(request.Id), request.Description));
        return new EmptyResponse();
    }
    
    public override async Task<EmptyResponse> UpdateWidget(UpdateWidgetRequest request, ServerCallContext context)
    {
        await _module.ExecuteCommandAsync(new UpdateWidget.Command(Guid.Parse(request.Id), request.Description));
        return new EmptyResponse();
    }

    public override async Task<GetWidgetResponse> GetWidget(GetWidgetRequest request, ServerCallContext context)
    {
        var result = await _module.ExecuteQueryAsync(new GetWidget.Query(Guid.Parse(request.Id)));
        return new GetWidgetResponse
        {
            Id = result.Id.ToString(),
            Description = result.Description ?? string.Empty
        };
    }
    public override async Task<ListWidgetsResponse> ListWidgets(ListWidgetsRequest request, ServerCallContext context)
    {
        var results = await _module.ExecuteQueryAsync(new ListWidgets.Query());

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