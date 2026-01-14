using System.Text.Json;
using Gaia.Models;
using Gaia.Services;
using Manis.Contract.Models;
using Manis.Contract.Services;

namespace Melnikov.Services;

public sealed class AuthenticationHttpService(
    HttpClient httpClient,
    JsonSerializerOptions options,
    ITryPolicyService tryPolicyService,
    IFactory<Memory<HttpHeader>> headersFactory
)
    : HttpService<ManisGetRequest, ManisPostRequest, ManisGetResponse, ManisPostResponse>(
        httpClient,
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
