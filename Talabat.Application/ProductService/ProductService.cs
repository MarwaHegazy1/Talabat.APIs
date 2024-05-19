using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;
using Talabat.Core.Services.Contract;

namespace Talabat.Application.ProductService
{
	public class ProductService : IProductService
	{
       /* public ProductService(IUnitOfWork unitOfWork)
        {
            
        }*/
        public Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Product> GetProductAsync(int productId)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Product>> GetProductsAsync()
		{
			throw new NotImplementedException();
		}
	}
}
