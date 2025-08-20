using Fujitsu.Challenge.API.Models.Auth;

namespace Fujitsu.Challenge.API.Interfaces
{
    public interface IAuthService
    {
        IResponse<LoginResponse> Login(LoginRequest request);
        IResponse<bool> Register(RegisterRequest input);
    }
}
