using System.Text.Json;
using Gaia.Services;
using Manis.Contract.Models;
using Manis.Contract.Services;

namespace Melnikov.Services;

public class AuthenticationService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions, ITryPolicyService tryPolicyService) : Service<ManisGetRequest, ManisPostRequest, ManisGetResponse, ManisPostResponse>(httpClient, jsonSerializerOptions, tryPolicyService), IAuthenticationService;