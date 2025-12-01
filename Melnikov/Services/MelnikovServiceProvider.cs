using System.Text.Json;
using Jab;
using Manis.Contract.Services;
using Melnikov.Models;
using ManisJsonContext = Manis.Contract.Models.ManisJsonContext;

namespace Melnikov.Services;

[ServiceProviderModule]
[Transient(typeof(IManisService), Factory = nameof(GetManisService))]
[Transient(typeof(JsonSerializerOptions), Factory = nameof(GetJsonSerializerOptions))]
public interface IMelnikovServiceProvider
{
    public static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new()
        {
            TypeInfoResolver = ManisJsonContext.Resolver,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    public static IManisService GetManisService(ManisServiceOptions options, JsonSerializerOptions jsonOptions)
    {
        return new ManisService(new()
        {
            BaseAddress = new(options.Url),
        }, jsonOptions);
    }
}