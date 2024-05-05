 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;

namespace Talabat.Application.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IGenericRepository<Product> _productRepo;
		private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
		private readonly IGenericRepository<Order> _orderRepo;

		public OrderService(IBasketRepository basketRepo
			, IGenericRepository<Product> productRepo
			, IGenericRepository<DeliveryMethod> deliveryMethodRepo
			, IGenericRepository<Order> orderRepo)
        {
			_basketRepo = basketRepo;
			_productRepo = productRepo;
			_deliveryMethodRepo = deliveryMethodRepo;
			_orderRepo = orderRepo;
		}
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
		{
			// 1.Get Basket From Baskets Repo
			var basket = await _basketRepo.GetBasketAsync(basketId);

			// 2. Get Selected Items at Basket From Products Repo
			var orderItems = new List<OrderItem>();

			if (basket?.Items?.Count > 0)
			{
				foreach (var item in basket.Items)
				{
					var product = await _productRepo.GetAsync(item.Id);
					
					var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);

					var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

					orderItems.Add(orderItem);
				}
			}

			// 3. Calculate SubTotal
			var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

			// 4. Get Delivery Method From DeliveryMethods Repo
			var deliveryMethod = await _deliveryMethodRepo.GetAsync(deliveryMethodId);

			// 5. Create Order
			var order = new Order(
				buyerEmail: buyerEmail,
				shippingAddress: shippingAddress,
				deliveryMethod: deliveryMethod,
				items: orderItems,
				subtotal: subtotal
				);

			_orderRepo.Add(order);

			// 6. Save To Database [TODO]

		}

		public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			throw new NotImplementedException();
		}
	}
}
