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
        public AuthService(StrivoTestContext context, ITokenGenerator tokenGenerator)
        {
            _context = context;
            _tokenGenerator = tokenGenerator;
        }



        public async Task<Response<LoginResponse>> LoginAsync(LoginModel login)
        {
            if(login == null)
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

            var service = await _context.Services.Where(x => x.ServiceId == login.Service_Id && x.IsActive).Select(
                x => new ServiceModel
                {
                    Name = x.Name,
                    IsActive = x.IsActive,
                    ServiceId = x.ServiceId,
                    PasswordHash = x.PasswordHash
                }).FirstOrDefaultAsync();
            

            if (service == null)
            {
                return new Response<LoginResponse>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "invalid service id or password"
                };
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                login.Password,
                service.PasswordHash
            );

            if (!isPasswordValid)
            {
                return new Response<LoginResponse>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "invalid service id or password"
                };
            }

            var token = _tokenGenerator.GenerateJwtToken(service);

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
                ServiceId = service.ServiceId,
                CreatedAt = token.CreatedAt,
                ExpiresAt = token.ExpiresAt,
                Token = token.Token,
            };
            var saveToken = await _context.ServiceTokens.AddAsync(serviceToken);
            var save = await _context.SaveChangesAsync();
            if(save > 0)
            {
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

            return new Response<LoginResponse>
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "an issue occured."
            };
        }
    } 
}
