using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrivoLabsTest.Data.DTOs.Services;
using StrivoLabsTest.Interfaces;

namespace StrivoLabsTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : BaseController
    {
        private readonly IServiceService _services;
        public ServiceController(IServiceService services)
        {
            _services = services;
        }


        [HttpPost("new")]
        public async Task<IActionResult> CreateService(ServiceDTO service)
        {
            var response = await _services.CreateService(service);
            return ReturnResponse(response);
        }

    }
}
