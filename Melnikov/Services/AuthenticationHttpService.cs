using System.Text.Json;
using Gaia.Models;
using Gaia.Services;
using Manis.Contract.Models;
using Manis.Contract.Services;

namespace Melnikov.Services;

public class AuthenticationHttpService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions, ITryPolicyService tryPolicyService, IFactory<Memory<HttpHeader>> headersFactory) : HttpService<ManisGetRequest, ManisPostRequest, ManisGetResponse, ManisPostResponse>(httpClient, jsonSerializerOptions, tryPolicyService, headersFactory), IAuthenticationService;