using Newtonsoft.Json;

namespace Demo.Infrastructure.Repositories.Serialisation;

internal class JsonHelper
{
    public static string ToJson(object o)
    {
        return JsonConvert.SerializeObject(o);
    }

    public static T? ToObject<T>(string? json) where T : class
    {
        return string.IsNullOrEmpty(json) 
            ? null 
            : JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ContractResolver = new JsonPrivateContractResolver()
            });
    }
}