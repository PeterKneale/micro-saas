namespace Modules.Settings;

internal static class SettingsCompositionRoot
{
    private static IServiceProvider? _provider;

    internal static void SetProvider(IServiceProvider container)
    {
        _provider = container;
    }

    internal static IServiceScope BeginLifetimeScope()
    {
        if (_provider == null)
        {
            throw new Exception("Module setup has not been performed");
        }
        return _provider.CreateScope();
    }
}