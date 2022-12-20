namespace Backend.Core.Infrastructure.Interceptors;

internal class MissingTenantHeaderException : Exception
{
    public MissingTenantHeaderException() : base("Tenant header not found")
    {
        
    }
}