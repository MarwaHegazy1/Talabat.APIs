using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs_02.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs_02.Controllers
{
	public class PaymentController : BaseApiController
	{
		private readonly IPaymentService _paymentService;
		private readonly ILogger<PaymentController> _logger;
		private const string whSecret = "whsec_c38562b0b8ff420f2727e197275701cc7274b04464c06d2f456d6e50658a6d25";
	
		public PaymentController(IPaymentService paymentService,ILogger<PaymentController> logger)
		{
			_paymentService = paymentService;
			_logger = logger;
		}

		[Authorize]
		[ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[HttpPost("{basketid}")]
		public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
			if (basket is null) return BadRequest(new ApiResponse(400, "An Error with your Basket"));
			return Ok(basket);
		}

		[HttpPost("webhook")]
		public async Task<IActionResult> WebHook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
			var stripeEvent = EventUtility.ConstructEvent(json,
				Request.Headers["Stripe-Signature"], whSecret);

			var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

			Order? order = null;
			switch (stripeEvent.Type)
			{
				case Events.PaymentIntentSucceeded:
					await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);

					_logger.LogInformation("Order is Succeeded {0}", order?.PaymentIntentId);
					_logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);

					break;
				case Events.PaymentIntentPaymentFailed:
					await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);

					_logger.LogInformation("Order is Failed {0}", order?.PaymentIntentId);
					_logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);

					break;
			}
			return Ok();
		}
	}
}
