using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrivoLabsTest.Data.DTOs.Login;
using StrivoLabsTest.Data.DTOs.Subscription;
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


        [HttpPut("unsubscribe")]
        public async Task<IActionResult> UnSubScribe(SubscribersRequest model)
        {
            var response = await _subService.UnSubscribe(model);
            return ReturnResponse(response);
        }



        [HttpPatch("status")]
        public async Task<IActionResult> Status(SubscribersRequest model)
        {
            var response = await _subService.CheckSubcriptionStatus(model);
            return ReturnResponse(response);
        }
    }
}
