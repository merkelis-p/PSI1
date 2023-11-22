﻿using System;
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


    private readonly ILogger<ApiService<T>> _logger;

    public ApiService(IConfiguration configuration, ILogger<ApiService<T>> logger)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(configuration["ApiBaseUrl"])
        };

        _logger = logger;
        _endpoint = $"api/{typeof(T).Name}";
    }


    //public async Task<IEnumerable<T>> GetAllAsync()
    //{
    //    var response = await _httpClient.GetAsync(_endpoint);
    //    response.EnsureSuccessStatusCode();

    //    var content = await response.Content.ReadAsStringAsync();
    //    return JsonSerializer.Deserialize<IEnumerable<T>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    //}

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(_endpoint);

            // You might want to check for specific HTTP status codes here
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"API call to {_endpoint} failed with status code {response.StatusCode}");
                // Handle specific status codes or throw a custom exception
                throw new HttpRequestException($"Request to API failed: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<T>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HttpRequestException occurred while calling API");
            throw; // Re-throwing the exception to be handled by the caller
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GetAllAsync");
            throw; // Re-throwing the exception to be handled by the caller
        }
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

        var jsonData = JsonSerializer.Serialize(entity);
        _logger.LogInformation($"Adding entity: {jsonData}");

        var content = new StringContent(JsonSerializer.Serialize(entity), System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_endpoint, content);

        if (!response.IsSuccessStatusCode) // Check if response status is not in the successful range (200-299)
        {
            var errorContent = await response.Content.ReadAsStringAsync();

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.InternalServerError:
                    throw new Exception($"API call failed with status code {response.StatusCode}. Error: {errorContent}");

                case System.Net.HttpStatusCode.BadRequest:
                    throw new Exception($"API call was bad request with status code {response.StatusCode}. Error: {errorContent}");

                // Add other specific status codes as needed.
            
                default:
                    throw new Exception($"API call failed with status code {response.StatusCode}. Error: {errorContent}");
            }
        }
        
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<bool> UpdateAsync(int id, T entity)
    {
        var jsonData = JsonSerializer.Serialize(entity);
        _logger.LogInformation($"Updating entity: {jsonData}");

        var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
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
        var response = await _httpClient.PostAsJsonAsync("api/User/Login", loginRequest);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new LoginValidationResult { IsValid = false };
        }
        return await response.Content.ReadAsAsync<LoginValidationResult>();
    }


}
