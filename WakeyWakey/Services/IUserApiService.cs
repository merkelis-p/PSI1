using WakeyWakey.Models;

namespace WakeyWakey.Services;

public interface IUserApiService : IApiService<User>
{

    public Task<LoginValidationResult> ValidateLogin(string username, string password);
    public Task<bool> ValidateRegister(string username, string email, string password);

}