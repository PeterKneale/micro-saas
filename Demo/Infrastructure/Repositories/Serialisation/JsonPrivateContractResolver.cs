using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demo.Infrastructure.Repositories.Serialisation;

public class JsonPrivateContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var prop = base.CreateProperty(member, memberSerialization);
        if (prop.Writable) return prop;

        var property = member as PropertyInfo;
        var hasPrivateSetter = property?.GetSetMethod(true) != null;
        prop.Writable = hasPrivateSetter;
        return prop;
    }
}