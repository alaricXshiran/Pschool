using System.Net.Http.Json;
using Pschool.Shared.Dtos;

namespace Pschool.Client.Services;

// All calls to the API's /api/students endpoints, in one place.
public class StudentService
{
    private readonly HttpClient _http;

    public StudentService(HttpClient http)
    {
        _http = http;
    }

    // One page of students, optionally for one parent and filtered by a search word.
    public async Task<PagedResult<StudentResponse>> GetPageAsync(
        int page, int pageSize, int? parentId = null, string? search = null)
    {
        var url = $"api/students?page={page}&pageSize={pageSize}";

        if (parentId.HasValue)
        {
            url += $"&parentId={parentId.Value}";
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            // Escape the text so spaces and symbols are safe inside an address.
            url += $"&search={Uri.EscapeDataString(search.Trim())}";
        }

        return await _http.GetFromJsonAsync<PagedResult<StudentResponse>>(url)
            ?? new PagedResult<StudentResponse>();
    }
}