using AutoMapper;
using Talabat.APIs_02.Dtos;
using Talabat.Core.Entities.Product;

namespace Talabat.APIs_02.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
	{
		private readonly IConfiguration _configuration;

		public ProductPictureUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.PictureUrl))
				return $"{_configuration["ApiBaseUrl"]}/{source.PictureUrl}";

			return string.Empty;
		}
	}
}
