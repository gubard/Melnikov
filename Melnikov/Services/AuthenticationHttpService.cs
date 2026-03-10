using System.Text.Json;
using Gaia.Models;
using Gaia.Services;
using Manis.Contract.Models;
using Manis.Contract.Services;

namespace Melnikov.Services;

public sealed class AuthenticationHttpService(
    IFactory<HttpClient> httpClientFactory,
    JsonSerializerOptions options,
    ITryPolicyService tryPolicyService,
    IFactory<Memory<HttpHeader>> headersFactory
)
    : HttpService<ManisGetRequest, ManisPostRequest, ManisGetResponse, ManisPostResponse>(
        httpClientFactory,
        options,
        tryPolicyService,
        headersFactory
    ),
        IAuthenticationService
{
    protected override ManisGetRequest CreateHealthCheckGetRequest()
    {
        return new();
    }
}
