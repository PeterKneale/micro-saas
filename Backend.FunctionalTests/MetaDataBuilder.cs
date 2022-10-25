using Grpc.Core;

namespace Backend.FunctionalTests;

internal class MetaDataBuilder
{
    public static Metadata WithTenant(string tenant) => new Metadata
    {
        {
            "tenant", tenant
        }
    };

}