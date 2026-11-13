using System.Net.Http.Json;
using Pschool.Shared.Dtos;

namespace Pschool.Client.Services;

// All calls to the API's /api/parents endpoints, in one place.
public class ParentService
{
    private readonly HttpClient _http;

    public ParentService(HttpClient http)
    {
        _http = http;
    }

    // One page of parents, optionally filtered by a search word.
    public async Task<PagedResult<ParentResponse>> GetPageAsync(int page, int pageSize, string? search = null)
    {
        var url = $"api/parents?page={page}&pageSize={pageSize}";

        if (!string.IsNullOrWhiteSpace(search))
        {
            // Escape the text so spaces and symbols are safe inside an address.
            url += $"&search={Uri.EscapeDataString(search.Trim())}";
        }

        return await _http.GetFromJsonAsync<PagedResult<ParentResponse>>(url)
            ?? new PagedResult<ParentResponse>();
    }
    // Send a new parent to the API.
    public async Task<ApiResult> CreateAsync(ParentRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/parents", request);
            return await ApiResult.FromResponseAsync(response);
        }
        catch (HttpRequestException)
        {
            return ApiResult.Fail("Could not reach the API. Check that it is running.");
        }
    }
}