using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WakeyWakey.Models;

namespace WakeyWakey.Services;


public class ApiService<T>:IApiService<T>
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;

    public ApiService(IConfiguration configuration)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(configuration["ApiBaseUrl"])
        };

        _endpoint = $"api/{typeof(T).Name}";
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var response = await _httpClient.GetAsync(_endpoint);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<T>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<T> GetByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/{id}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<T> AddAsync(T entity)
    {
        var content = new StringContent(JsonSerializer.Serialize(entity), System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_endpoint, content);
        if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"API call failed with status code {response.StatusCode}");
        }

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<bool> UpdateAsync(int id, T entity)
    {
        var content = new StringContent(JsonSerializer.Serialize(entity), System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{_endpoint}/{id}", content);
        response.EnsureSuccessStatusCode();

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_endpoint}/{id}");
        return response.IsSuccessStatusCode;
    }


    public async Task<LoginValidationResult> ValidateLogin(string username, string password)
    {
        var loginRequest = new UserLoginRequest { Username = username, Password = password };
        var response = await _httpClient.PostAsJsonAsync("api/Users/Login", loginRequest);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new LoginValidationResult { IsValid = false };
        }
        return await response.Content.ReadAsAsync<LoginValidationResult>();
    }


}
