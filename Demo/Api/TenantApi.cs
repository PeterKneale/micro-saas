using Demo.Application.Commands;
using Demo.Application.Queries;
using Grpc.Core;

namespace Demo.Api;

public class TenantApi : TenantService.TenantServiceBase
{
    private readonly IMediator _mediator;

    public TenantApi(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<EmptyResponse> AddCar(AddCarRequest request, ServerCallContext context)
    {
        await _mediator.Send(new AddCar.Command(Guid.Parse(request.Id)));
        return new EmptyResponse();
    }
    public override async Task<EmptyResponse> RegisterCar(RegisterCarRequest request, ServerCallContext context)
    {
        await _mediator.Send(new RegisterCar.Command(Guid.Parse(request.Id), request.Registration));
        return new EmptyResponse();
    }

    public override async Task<GetCarResponse> GetCar(GetCarRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetCar.Query(Guid.Parse(request.Id)));
        return new GetCarResponse
        {
            Id = result.Id.ToString(),
            Registration = result.Registration ?? string.Empty
        };
    }

    public override async Task<GetCarByRegistrationResponse> GetCarByRegistration(GetCarByRegistrationRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetCarByRegistration.Query(request.Registration));
        return new GetCarByRegistrationResponse
        {
            Id = result.Id.ToString(),
            Registration = result.Registration
        };
    }

    public class GetCarValidator : AbstractValidator<GetCarRequest>
    {
        public GetCarValidator()
        {
            RuleFor(x => x.Id).NotNull()
                .NotEmpty()
                .Must(x => Guid.TryParse(x, out _))
                .WithMessage("'Id' must be a valid GUID");
        }
    }

    public class GetCarByRegistrationValidator : AbstractValidator<GetCarByRegistrationRequest>
    {
        public GetCarByRegistrationValidator()
        {
            RuleFor(x => x.Registration)
                .NotNull()
                .NotEmpty();
        }
    }
}