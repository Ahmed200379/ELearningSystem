using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Chat;
using Shared.Dtos.Payment;
using Stripe;

namespace ELearningSystem.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;
        private readonly IConfiguration _config;
        public PaymentController(IPaymentServices paymentServices,IConfiguration configuration)
        {
            _paymentServices = paymentServices;
            _config = configuration;
        }
        [HttpPost("api/CreatePayent")]
        public async Task<IActionResult> CreatePayent(CreatePaymentDto createPaymentDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState.SelectMany(e => e.Value.Errors).Select(er => er.ErrorMessage));
            }
            try
            {
                var result = await _paymentServices.CreatePayment(createPaymentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("api/VerifyPayment")]
        public async Task<IActionResult> VerifyPayment()
        {
            try
            {
                var json = await new StreamReader(Request.Body).ReadToEndAsync();
                var signature = Request.Headers["Stripe-Signature"];
                var StripeEvent = EventUtility.ConstructEvent(
                    json,
                    signature,
                    _config["Stripe:WebhookSecret"]
                );
               var result= await _paymentServices.VerifyPayment(StripeEvent);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
