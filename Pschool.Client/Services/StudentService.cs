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

    // Send a new student to the API.
    public async Task<ApiResult> CreateAsync(StudentRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/students", request);
            return await ApiResult.FromResponseAsync(response);
        }
        catch (HttpRequestException)
        {
            return ApiResult.Fail("Could not reach the API. Check that it is running.");
        }
    }

    // Send changed details for an existing student.
    public async Task<ApiResult> UpdateAsync(int id, StudentRequest request)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"api/students/{id}", request);
            return await ApiResult.FromResponseAsync(response);
        }
        catch (HttpRequestException)
        {
            return ApiResult.Fail("Could not reach the API. Check that it is running.");
        }
    }

    // Delete a student.
    public async Task<ApiResult> DeleteAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/students/{id}");
            return await ApiResult.FromResponseAsync(response);
        }
        catch (HttpRequestException)
        {
            return ApiResult.Fail("Could not reach the API. Check that it is running.");
        }
    }
}