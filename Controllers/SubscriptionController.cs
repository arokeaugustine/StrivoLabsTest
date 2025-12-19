using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrivoLabsTest.Data.DTOs;
using StrivoLabsTest.Data.DTOs.Login;
using StrivoLabsTest.Interfaces;

namespace StrivoLabsTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : BaseController
    {
        private readonly ISubscriptionService _subService;
        public SubscriptionController(ISubscriptionService subService)
        {
            _subService = subService;
        }


        [HttpPost("subscribe")]
        public async Task<IActionResult> SubScribe(SubscribersRequest model)
        {
            var response = await _subService.Subscribe(model);
            return ReturnResponse(response);
        }


        [HttpPost("unsubscribe")]
        public async Task<IActionResult> UnSubScribe(SubscribersRequest model)
        {
            var response = await _subService.UnSubscribe(model);
            return ReturnResponse(response);
        }
    }
}
