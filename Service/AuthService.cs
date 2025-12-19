using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using StrivoLabsTest.Data.DTOs;
using StrivoLabsTest.Data.DTOs.Login;
using StrivoLabsTest.Data.DTOs.Services;
using StrivoLabsTest.Data.Models;
using StrivoLabsTest.Interfaces;
using System.Net;


namespace StrivoLabsTest.Service
{
    public class AuthService : IAuthService
    {
        private readonly StrivoTestContext _context;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IServiceService _serviceService;
        public AuthService(StrivoTestContext context,
            ITokenGenerator tokenGenerator,
            IServiceService serviceService)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
            _serviceService = serviceService;
        }



        public async Task<Response<LoginResponse>> LoginAsync(LoginModel login)
        {

            var validateLogin = ValidateLogin(login);
            if (!validateLogin.IsSuccess)
            {
                return validateLogin;
            }

            var service = await _serviceService.GetActiveService(login.Service_Id);

            if (!service.IsSuccess)
            {
                return new Response<LoginResponse>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "invalid service id or password"
                };
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify( login.Password, service.Data.PasswordHash);

            if (!isPasswordValid)
            {
                return new Response<LoginResponse>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "invalid service id or password"
                };
            }

            var token = _tokenGenerator.GenerateJwtToken(service.Data);

            if (string.IsNullOrEmpty(token.Token))
            {
                return new Response<LoginResponse>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "unable to generate auth token"
                };
            }
            var serviceToken = new ServiceToken
            {
                ServiceId = service.Data.ServiceId,
                CreatedAt = token.CreatedAt,
                ExpiresAt = token.ExpiresAt,
                Token = token.Token,
            };

            return await SaveToken(service.Data, serviceToken);
        }


        private Response<LoginResponse> ValidateLogin(LoginModel login)
        {
            if (login == null)
            {
                return new Response<LoginResponse>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid data"
                };
            }

            if (string.IsNullOrWhiteSpace(login.Password))
            {
                return new Response<LoginResponse>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "password is required"
                };
            }
            return new Response<LoginResponse>
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = "success"
            };

        }

        private async Task<Response<LoginResponse>> SaveToken(ServiceModel service, ServiceToken serviceToken)
        {
          
            var saveToken = await _context.ServiceTokens.AddAsync(serviceToken);
            var save = await _context.SaveChangesAsync();

            return new Response<LoginResponse>
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = "successful",
                Data = new LoginResponse
                {
                    Token = serviceToken.Token,
                    Name = service.Name,
                    TokenID = serviceToken.Uid

                }
            };
        }
    } 
}
