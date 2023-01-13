using Backend.Modules.Settings;
using Backend.Modules.Settings.Application.Commands;
using Backend.Modules.Settings.Application.Queries;

namespace Backend.Api;

public class SettingsApiService : SettingsApi.SettingsApiBase
{
    private readonly ISettingsModule _module;

    public SettingsApiService(ISettingsModule module)
    {
        _module = module;
    }

    public override async Task<GetThemeResponse> GetTheme(GetThemeRequest request, ServerCallContext context)
    {
        var theme = await _module.ExecuteQueryAsync(new GetTheme.Query());
        return new GetThemeResponse
        {
            Theme = theme
        };
    }

    public override async Task<EmptyResponse4> SetTheme(SetThemeRequest request, ServerCallContext context)
    {
        await _module.ExecuteCommandAsync(new SetTheme.Command(request.Theme));
        return new EmptyResponse4();
    }

    public override async Task<EmptyResponse4> ResetTheme(ResetThemeRequest request, ServerCallContext context)
    {
        await _module.ExecuteCommandAsync(new ResetTheme.Command());
        return new EmptyResponse4();
    }
}