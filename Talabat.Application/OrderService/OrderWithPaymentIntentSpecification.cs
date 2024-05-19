using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Specifications;

namespace Talabat.Application.OrderService
{

	public class OrderWithPaymentIntentSpecification : BaseSpecifications<Order>
	{
		public OrderWithPaymentIntentSpecification(string? paymentIntentId)
		: base(O => O.PaymentIntentId == paymentIntentId)
		{

		}
	}
}