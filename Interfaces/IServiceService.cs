using StrivoLabsTest.Data.DTOs;
using StrivoLabsTest.Data.DTOs.Services;

namespace StrivoLabsTest.Interfaces
{
    public interface IServiceService
    {
        Task<Response<ServiceResponse>> CreateService(ServiceDTO data);
    }
}
