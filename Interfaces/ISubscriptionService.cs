using StrivoLabsTest.Data.DTOs;
using StrivoLabsTest.Data.DTOs.Subscription;

namespace StrivoLabsTest.Interfaces
{
    public interface ISubscriptionService
    {
        Task<Response<string>> Subscribe(SubscribersRequest model);
        Task<Response<string>> UnSubscribe(SubscribersRequest model);
        Task<Response<SubscriptionStatus>> CheckSubcriptionStatus(SubscribersRequest model);
    }
}
