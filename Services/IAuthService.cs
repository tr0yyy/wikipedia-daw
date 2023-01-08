using FluentResults;
using WikipediaDAW.RequestModels;

namespace WikipediaDAW.Services
{
    public interface IAuthService
    {
        Task<Result<string>> Register(RegisterRequest register);
        Task<Result<string>> Login(LoginRequest login);
    }
}
