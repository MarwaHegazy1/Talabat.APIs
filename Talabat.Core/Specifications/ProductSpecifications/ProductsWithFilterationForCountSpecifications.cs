using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;

namespace Talabat.Core.Specifications.ProductSpecifications
{
    public class ProductsWithFilterationForCountSpecifications : BaseSpecifications<Product>
	{
        public ProductsWithFilterationForCountSpecifications( ProductSpecParams specParams)
            : base(P =>
						(string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
						(!specParams.BrandId.HasValue || P.ProductBrandId == specParams.BrandId.Value) &&
						(!specParams.CategoryId.HasValue || P.ProductCategoryId == specParams.CategoryId.Value)
			)
        {
            
        }
    }
}
