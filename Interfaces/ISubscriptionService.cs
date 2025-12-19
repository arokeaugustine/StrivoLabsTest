using StrivoLabsTest.Data.DTOs;

namespace StrivoLabsTest.Interfaces
{
    public interface ISubscriptionService
    {
        Task<Response<string>> Subscribe(SubscribersRequest model);
        Task<Response<string>> UnSubscribe(SubscribersRequest model);
    }
}
