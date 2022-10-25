namespace Demo.Infrastructure.Interceptors;

internal class MissingTenantHeaderException : Exception
{
    public MissingTenantHeaderException() : base("Tenant header not found")
    {
        
    }
}