using Grpc.Core;

namespace Demo.FunctionalTests;

internal class MetaDataBuilder
{
    public static Metadata WithTenant(string tenant) => new Metadata
    {
        {
            "tenant", tenant
        }
    };

}