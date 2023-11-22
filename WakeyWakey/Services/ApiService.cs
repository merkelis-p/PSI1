using System.Text.Json;

namespace WakeyWakey.Services;

public class ApiService<T>:IApiService<T>
{
    protected readonly HttpClient _httpClient;
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
    
}
