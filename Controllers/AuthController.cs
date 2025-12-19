using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrivoLabsTest.Data.DTOs.Login;
using StrivoLabsTest.Data.DTOs.Services;
using StrivoLabsTest.Interfaces;

namespace StrivoLabsTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("login")]
        public async Task<IActionResult> CreateService(LoginModel login)
        {
            var response = await _authService.LoginAsync(login);
            return ReturnResponse(response);
        }

    }
}
