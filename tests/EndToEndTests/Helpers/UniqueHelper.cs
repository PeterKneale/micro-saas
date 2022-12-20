namespace EndToEndTests.Helpers;

public static class UniqueHelper
{
    public static string GetUniqueEmail() => "email-" + Guid.NewGuid().ToString()[..6] + "@example.com";
    public static string GetUniqueName() => "name-" + Guid.NewGuid().ToString()[..6];
    public static string GetUniqueIdentifier() => "identifier-" + Guid.NewGuid().ToString()[..6];
}