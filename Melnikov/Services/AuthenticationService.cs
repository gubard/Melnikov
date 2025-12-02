using System.Net.Http.Json;
using System.Text.Json;
using Gaia.Helpers;
using Manis.Contract.Models;
using Manis.Contract.Services;
using Zeus.Helpers;

namespace Melnikov.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public AuthenticationService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async ValueTask<ManisGetResponse> GetAsync(ManisGetRequest request, CancellationToken ct)
    {
        using var httpResponse = await _httpClient.PostAsJsonAsync(RouteHelper.Get, request, _jsonSerializerOptions, ct);
        var response = await httpResponse.Content.ReadFromJsonAsync<ManisGetResponse>(_jsonSerializerOptions, ct);

        return response.ThrowIfNull();
    }

    public async ValueTask<ManisPostResponse> PostAsync(ManisPostRequest request, CancellationToken ct)
    {
        using var httpResponse = await _httpClient.PostAsJsonAsync(RouteHelper.Post, request, _jsonSerializerOptions, ct);
        var response = await httpResponse.Content.ReadFromJsonAsync<ManisPostResponse>(_jsonSerializerOptions, ct);

        return response.ThrowIfNull();
    }
}