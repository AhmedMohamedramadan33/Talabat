using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Api.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Order_Aggregate;
using Talabat.Repository.Data.Repositories;
using Talabat.Service;

namespace Talabat.Api.Controllers
{

    public class PaymentsController :BaseApiController
    {
        private readonly IpaymentService ipaymentService;
        private const string WhSecret = "your_webhook_secret";
        public PaymentsController(IpaymentService ipaymentService)
        {
            this.ipaymentService = ipaymentService;
        }
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await ipaymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400,"An Error with Your basket"));
            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {          
                var json = await new StreamReader(Request.Body).ReadToEndAsync();
                Event stripeEvent;           
                // Validate the webhook signature
                stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WhSecret);
              var  paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                Order order;

                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                order= await ipaymentService.Updatepaymentintendtosuccedorfailed(paymentIntent.Id, true);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        order = await ipaymentService.Updatepaymentintendtosuccedorfailed(paymentIntent.Id, false);
                        break;
                    default:
                        Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                        break;
                }
            return Ok(); // Return a 200 response to acknowledge receipt of the event
            }
        }
    }
    

