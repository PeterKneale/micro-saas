using Backend.Modules.Registrations;
using Backend.Modules.Registrations.Application.Commands;

namespace Backend.Api;

public class RegistrationsApiService : RegistrationsApi.RegistrationsApiBase
{
    private readonly IRegistrationsModule _module;

    public RegistrationsApiService(IRegistrationsModule module)
    {
        _module = module;
    }

    public override async Task<EmptyResponse5> Register(RegisterRequest request, ServerCallContext context)
    {
        await _module.ExecuteCommandAsync(new Register.Command(request.Email, request.Name, request.Identifier, request.OverrideToken));
        return new EmptyResponse5();
    }

    public override async Task<EmptyResponse5> Claim(ClaimRequest request, ServerCallContext context)
    {
        await _module.ExecuteCommandAsync(new Claim.Command(request.Identifier, request.Password, request.Token));
        return new EmptyResponse5();
    }
}