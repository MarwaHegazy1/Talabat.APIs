using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecifications
{
	public class ProductWithBrandAndCategorySpecifications: BaseSpecifications<Product>
	{

		// This Constructor will be Used for Creating an Object, That will be Used to Get All Products
		public ProductWithBrandAndCategorySpecifications(string sort) : base()
		{
			AddIncludes();

			if (!string.IsNullOrEmpty(sort))
			{
				switch (sort)
				{
					case "priceAsc":
						AddOrderBy(P => P.Price);
						break;
					case "priceDesc":
						AddOrderByDesc(P => P.Price);
						break;
					default:
						AddOrderBy(P => P.Name);
						break;
				}
			}
			else
				AddOrderBy(P => P.Name);
		}

		// This Constructor will be Used for Creating an Object, That will be Used to Get a Specific Product with ID
		public ProductWithBrandAndCategorySpecifications(int id) : base(P => P.Id == id)
		{
			AddIncludes();
		}

		private void AddIncludes()
		{
			Includes.Add(P => P.Brand);
			Includes.Add(P => P.Category);
		}
	}
}
