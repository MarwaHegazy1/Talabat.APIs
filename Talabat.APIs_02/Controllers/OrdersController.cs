using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs_02.Dtos;
using Talabat.APIs_02.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs_02.Controllers
{

	public class OrdersController : BaseApiController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrdersController(IOrderService orderService,IMapper mapper)
        {
			_orderService = orderService;
			_mapper = mapper;
		}

		[ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
		{
			var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
			var order = await _orderService.CreateOrderAsync(orderDto.BuyerEmail, orderDto.DeliveryMethodId, orderDto.BasketId, address);

			if (order is null) return BadRequest(new ApiResponse(400));
			return Ok(order);
		}

		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser(string email)
		{
			var orders = await _orderService.GetOrdersForUserAsync(email);
			return Ok(orders);
		}
	}
}
