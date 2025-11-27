using Jab;
using Manis.Contract.Services;
using Melnikov.Models;
using ManisJsonContext = Manis.Contract.Models.ManisJsonContext;

namespace Melnikov.Services;

[ServiceProviderModule]
[Transient(typeof(IManisService), Factory = nameof(GetManisService))]
public interface IMelnikovServiceProvider
{
    public static IManisService GetManisService(ManisServiceOptions options)
    {
        return new ManisService(new()
        {
            BaseAddress = new(options.Url),
        }, new ManisJsonContext().Options);
    }
}