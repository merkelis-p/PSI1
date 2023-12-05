using WakeyWakey.Models;

namespace WakeyWakey.Services;

public class UserApiService : ApiService<User>, IUserApiService
{
    
    public UserApiService(IConfiguration configuration, ILogger<UserApiService> logger) : base(configuration, logger)
    {
        
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
    
    // Validate Register
    public async Task<bool> ValidateRegister(string username, string email, string password)
    {
        var registerRequest = new UserRegisterRequest { Username = username, Email = email, Password = password};
        var response = await _httpClient.PostAsJsonAsync("api/User/Register", registerRequest);
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            return false;
        }
        return true;
    }
    
    
    
}