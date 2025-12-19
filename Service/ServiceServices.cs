using Microsoft.EntityFrameworkCore;
using StrivoLabsTest.Data.DTOs;
using StrivoLabsTest.Data.DTOs.Services;
using StrivoLabsTest.Data.Models;
using StrivoLabsTest.Interfaces;
using System.Net;

namespace StrivoLabsTest.Service
{
    public class ServiceServices : IServiceService
    {

        private readonly StrivoTestContext _context;
        public ServiceServices(StrivoTestContext context)
        {
            _context = context;
        }


        public async Task<Response<ServiceResponse>> CreateService(ServiceDTO data)
        {
            if (data == null)
            {
                return new Response<ServiceResponse>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "invalid data"
                };
            }

            if (string.IsNullOrWhiteSpace(data.Name) || string.IsNullOrWhiteSpace(data.PasswordHash))
            {
                return new Response<ServiceResponse>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "name and password of service is required"
                };
            }

            var serviceCheck = await _context.Services.AnyAsync(x => x.Name.ToLower() == data.Name.ToLower());
            if (serviceCheck)
            {
                return new Response<ServiceResponse>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "service already exist"
                };
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(data.PasswordHash);

            var newService = new StrivoLabsTest.Data.Models.Service
            {

                Name = data.Name,
                ServiceId = Guid.NewGuid(),
                PasswordHash = hashedPassword,
                IsActive = true
            };

            await _context.Services.AddAsync(newService);
            var save = await _context.SaveChangesAsync();

            if (save > 0)
            {
                return new Response<ServiceResponse>
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "service added successful",
                    Data = new ServiceResponse
                    {
                        IsActive = newService.IsActive,
                        ServiceId = newService.ServiceId,
                        Name = newService.Name
                    }
                };
            }

            return new Response<ServiceResponse>
            {
                IsSuccess = false,
                Message = "service added successful"
            };

        }

    }
}
