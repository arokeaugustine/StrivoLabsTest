using Microsoft.EntityFrameworkCore;
using StrivoLabsTest.Data.DTOs;
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
            if (string.IsNullOrWhiteSpace(model.Phone_number))
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid phone number"
                };
            }

            var serviceToken = await _context.ServiceTokens.FirstOrDefaultAsync(x => x.Uid == model.Token_id);
            if (serviceToken is null)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid token"
                };
            }

            if (serviceToken.ExpiresAt < DateTime.UtcNow)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "token expired"
                };
            }

            var subscriber = new Subscriber
            {
                ServiceId = model.Service_id,
                SubscribedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                PhoneNumber = model.Phone_number,
                IsSubscribed = true
            };

            await _context.Subscribers.AddAsync(subscriber);
            var save = await _context.SaveChangesAsync();

            if (save > 0)
            {
                return new Response<string>
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "subscription successful"
                };
            }

            return new Response<string>
            {
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "unable to sunscribe"
            };

        }


        public async Task<Response<string>> UnSubscribe(SubscribersRequest model)
        {
            if (string.IsNullOrWhiteSpace(model.Phone_number))
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid phone number"
                };
            }

            var serviceToken = await _context.ServiceTokens.FirstOrDefaultAsync(x => x.Uid == model.Token_id);
            if (serviceToken is null)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Invalid token"
                };
            }

            if (serviceToken.ExpiresAt < DateTime.UtcNow)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "token expired"
                };
            }

            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.PhoneNumber == model.Phone_number && x.ServiceId == model.Service_id);
            if (subscriber is null)
            {
                return new Response<string>
                {
                    IsSuccess = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "unable to subscribe"
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
            subscriber.SubscribedAt = DateTime.UtcNow;
            

            var save = await _context.SaveChangesAsync();

            if (save > 0)
            {
                return new Response<string>
                {
                    IsSuccess = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "unsubscription successful"
                };
            }

            return new Response<string>
            {
                IsSuccess = false,
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "unable to sunscribe"
            };

        }



    }
}
