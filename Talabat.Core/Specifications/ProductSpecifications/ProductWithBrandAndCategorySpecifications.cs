﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;

namespace Talabat.Core.Specifications.ProductSpecifications
{
    public class ProductWithBrandAndCategorySpecifications: BaseSpecifications<Product>
	{

		// This Constructor will be Used for Creating an Object, That will be Used to Get All Products
		public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams)
			: base(P =>
			(string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
			(!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId.Value) &&
			(!specParams.CategoryId.HasValue || P.ProductCategoryId == specParams.CategoryId.Value)
			)
		{
			AddIncludes();

			if (!string.IsNullOrEmpty(specParams.Sort))
			{
				switch (specParams.Sort)
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

			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
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
