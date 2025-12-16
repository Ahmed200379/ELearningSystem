using Microsoft.Extensions.Logging;
using Shared.Dtos;
using Shared.Dtos.Payment;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IPaymentServices
    {
        Task<GeneralResponseDto> CreatePayment(CreatePaymentDto createPaymentDto);
        Task<GeneralResponseDto> VerifyPayment(Event stripeEvent);
    }
}
