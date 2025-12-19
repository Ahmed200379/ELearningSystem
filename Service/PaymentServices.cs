using Domain.Entities;
using Domain.Interfaces;
using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using Services.Abstractions;
using Shared.Dtos;
using Shared.Dtos.Payment;
using Shared.Dtos.Subscribe;
using Shared.Enums;
using Stripe;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly ISubscribtionServices _subscribtionServices;
        public PaymentServices(IUnitOfWork unitOfWork,IConfiguration configuration, ISubscribtionServices subscribtionServices)
        {
            _unitOfWork = unitOfWork;
            _config = configuration;
            _subscribtionServices = subscribtionServices;
        }
        public async Task<GeneralResponseDto> CreatePayment(CreatePaymentDto createPaymentDto)
        {
             Expression<Func<UserGroup,Object>>[] include = { us=>us.Group };
            var student = await _unitOfWork.GetRepository<UserGroup>().GetFirstOrDefault(predicate:ug=>ug.UserId==createPaymentDto.StudentId && ug.GroupId==createPaymentDto.GroupId,includes:include);
            if (student == null)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Student not found in the specified group.",
                };
            }
            var paymentRecord = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                UserId = createPaymentDto.StudentId,
                GroupId = createPaymentDto.GroupId,
                Amount = student.Group.SubscriptionFee,
                CreatedAt = DateTime.UtcNow,
                Method = Shared.Enums.PaymentMethod.VesaPay,
                Status = PaymentStatus.Pending
            };
            var options = new PaymentIntentCreateOptions
            {
                    Amount=(long)(1000* 100),
                    Currency=createPaymentDto.Currency,
                    AutomaticPaymentMethods = new()
                    {
                        Enabled= true,
                    },
                     Metadata = new Dictionary<string, string>
                     {
                         { "userId",createPaymentDto.StudentId},
                         { "groupId",createPaymentDto.GroupId},
                         { "paymentId",paymentRecord.Id}

                     }
            };
            var service = new PaymentIntentService();
            PaymentIntent paymentIntent = await service.CreateAsync(options);
            await _unitOfWork.GetRepository<Payment>().AddAsync(paymentRecord);
            var result=await _unitOfWork.SaveChanges();
            if (result==0)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Failed to create payment record.",
                };
            }
            Console.WriteLine(paymentIntent.Id);
            return new GeneralResponseDto
            {
                IsSuccess = true,
                message = "Payment intent created successfully.",
                data = new
                {
                    ClientSecret = paymentIntent.ClientSecret
                }
            };
        }

        public async Task<GeneralResponseDto> VerifyPayment(Event stripeEvent)
        {
            
            string userId;
            string groupId;
            string paymentId;
            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (!paymentIntent.Metadata.TryGetValue("userId", out userId) ||
               !paymentIntent.Metadata.TryGetValue("groupId", out groupId) ||
              !paymentIntent.Metadata.TryGetValue("paymentId", out paymentId))
                {
                    return new GeneralResponseDto
                    {
                        IsSuccess = false,
                        message = "Metadata missing in payment intent."
                    };
                }
                var payment = await _unitOfWork.GetRepository<Payment>().GetFirstOrDefault(p => p.UserId == userId && p.GroupId == groupId && p.Id ==paymentId);
                payment.Status = PaymentStatus.Completed;
                _unitOfWork.GetRepository<Payment>().Update(payment);
                var subscribe = new UpdateSubscribeDto
                {
                    DurationInMonths = 1,
                    UserId = userId,
                    GroupId = groupId
                };
                await _subscribtionServices.UpdateSubscribeManually(subscribe);
                var result=await _unitOfWork.SaveChanges();
                if (result==0)
                {
                    return new GeneralResponseDto
                    {
                        IsSuccess = false,
                        message = "Failed to save payment record.",
                    };
                }

                return new GeneralResponseDto
                {
                    IsSuccess = true,
                    message = "Payment verified successfully.",
                };
            }
            else if (stripeEvent.Type == "payment_intent.payment_failed")
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                if (!paymentIntent.Metadata.TryGetValue("userId", out  userId) ||
                !paymentIntent.Metadata.TryGetValue("groupId", out  groupId) ||
               !paymentIntent.Metadata.TryGetValue("paymentId", out paymentId))
                {
                    return new GeneralResponseDto
                    {
                        IsSuccess = false,
                        message = "Metadata missing in payment intent."
                    };
                }
                var payment = await _unitOfWork.GetRepository<Payment>().GetFirstOrDefault(p => p.UserId == userId && p.GroupId == groupId && p.Id == paymentId);
                payment.Status = PaymentStatus.Canceled;
                _unitOfWork.GetRepository<Payment>().Update(payment);
                var result = await _unitOfWork.SaveChanges();
                if (result == 0)
                {
                    return new GeneralResponseDto
                    {
                        IsSuccess = false,
                        message = "Failed to save payment record.",
                    };
                }
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Payment verification failed.",
                };
            }
            else
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    message = "Unhandled event type.",
                };
            }
        }
    }
}
