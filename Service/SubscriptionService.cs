using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StrivoLabsTest.Data.DTOs;
using StrivoLabsTest.Data.DTOs.Subscription;
using StrivoLabsTest.Data.Models;
using StrivoLabsTest.Interfaces;
using System.Net;

namespace StrivoLabsTest.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly StrivoTestContext _context;
        public SubscriptionService(StrivoTestContext context)
        {
            _context = context;
        }

        public async Task<Response<string>> Subscribe(SubscribersRequest model)
        {
            if (!IsValidPhoneNumber(model.Phone_number))
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid phone number"
                };
            }
            var str = model.Phone_number.ToUpper();
            var token = await ValidateTokenAsync(model.Token_id);
            if (token == null)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid or expired token"
                };
            }

            var existingSubscriber = await _context.Subscribers
                .FirstOrDefaultAsync(x =>
                    x.PhoneNumber == model.Phone_number &&
                    x.ServiceId == model.Service_id &&
                    x.IsSubscribed);

            if (existingSubscriber != null)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "user already subscribed"
                };
            }

            var subscriber = new Subscriber
            {
                ServiceId = model.Service_id,
                PhoneNumber = model.Phone_number,
                IsSubscribed = true,
                SubscribedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Subscribers.AddAsync(subscriber);
            await _context.SaveChangesAsync();
            return new Response<string>
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = "subscription successful"
            };
        }

        public async Task<Response<string>> UnSubscribe(SubscribersRequest model)
        {
            if (!IsValidPhoneNumber(model.Phone_number))
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid phone number"
                };
            }
 

            var token = await ValidateTokenAsync(model.Token_id);
            if (token == null)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid or expired token"
                };
            }

            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(x =>
                    x.PhoneNumber == model.Phone_number &&
                    x.ServiceId == model.Service_id);

            if (subscriber is null)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "invalid subscription"
                };
            }

            if (!subscriber.IsSubscribed)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "service already unscribed"
                };
            }
            subscriber.IsSubscribed = false;
            subscriber.UnsubscribedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new Response<string>
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = "unsubscription successful"
            };
        }



        public async Task<Response<SubscriptionStatus>> CheckSubcriptionStatus(SubscribersRequest model)
        {
            if (!IsValidPhoneNumber(model.Phone_number))
            {
                return new Response<SubscriptionStatus>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid phone number"
                };
            }


            var token = await ValidateTokenAsync(model.Token_id);
            if (token == null)
            {
                return new Response<SubscriptionStatus>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid or expired token"
                };
            }

            var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(x =>
                    x.PhoneNumber == model.Phone_number &&
                    x.ServiceId == model.Service_id);

            if (subscriber is null)
            {
                return new Response<SubscriptionStatus>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "invalid subscription"
                };
            }

            if (!subscriber.IsSubscribed)
            {
                return new Response<SubscriptionStatus>
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "service unsubscribed",
                    Data = new SubscriptionStatus
                    {
                        Status = "Unsubscribed",
                        IsSubScribed = false,
                        UnsubscribedAt = subscriber.UnsubscribedAt
                    }
                };
            }

            return new Response<SubscriptionStatus>
            {
                IsSuccess = true,
                StatusCode = (int)HttpStatusCode.OK,
                Message = "service subscribed",
                Data = new SubscriptionStatus
                {
                    IsSubScribed = true,
                    Status = "Subscribed",
                    SubscribedAt = subscriber.SubscribedAt
                }
            };
        }


        private bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            return phoneNumber.All(char.IsDigit) &&
                   phoneNumber.Length >= 10 &&
                   phoneNumber.Length <= 15;
        }


        private async Task<ServiceToken?> ValidateTokenAsync(Guid tokenId)
        {
            var token = await _context.ServiceTokens
                .FirstOrDefaultAsync(x => x.Uid == tokenId);

            if (token == null)
                return null;

            if (token.ExpiresAt < DateTime.UtcNow)
                return null;

            return token;
        }


    }
}
