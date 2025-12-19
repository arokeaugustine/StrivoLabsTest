using StrivoLabsTest.Data.DTOs;
using StrivoLabsTest.Data.DTOs.Login;
using StrivoLabsTest.Data.DTOs.Services;

namespace StrivoLabsTest.Interfaces
{
    public interface IAuthService
    {
        Task<Response<LoginResponse>> LoginAsync(LoginModel login);
    }
}
