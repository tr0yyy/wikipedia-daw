using WikipediaDAW.RequestModels;

namespace WikipediaDAW.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegisterRequest register);
        Task<string> Login(LoginRequest login);
    }
}
