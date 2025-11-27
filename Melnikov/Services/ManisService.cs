using System.Net.Http.Json;
using System.Text.Json;
using Gaia.Helpers;
using Manis.Contract;
using Manis.Contract.Models;
using Manis.Contract.Services;
using Zeus.Helpers;

namespace Melnikov.Services;

public class ManisService : IManisService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public ManisService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
    {
        _httpClient = httpClient;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async ValueTask<ManisGetResponse> GetAsync(ManisGetRequest request, CancellationToken ct)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync(RouteHelper.Get, request, _jsonSerializerOptions, ct);
        var response = await httpResponse.Content.ReadFromJsonAsync<ManisGetResponse>(_jsonSerializerOptions, ct);

        return response.ThrowIfNull();
    }

    public async ValueTask<ManisPostResponse> PostAsync(ManisPostRequest request, CancellationToken ct)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync(RouteHelper.Post, request, _jsonSerializerOptions, ct);
        var response = await httpResponse.Content.ReadFromJsonAsync<ManisPostResponse>(_jsonSerializerOptions, ct);

        return response.ThrowIfNull();
    }
}