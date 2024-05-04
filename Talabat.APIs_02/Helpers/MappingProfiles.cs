using AutoMapper;
using Talabat.APIs_02.Dtos;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Product;

namespace Talabat.APIs_02.Helpers
{
    public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Product, ProductToReturnDto>()
				.ForMember(P => P.Brand, O => O.MapFrom(S => S.Brand.Name))
				.ForMember(P => P.Category, O => O.MapFrom(S => S.Category.Name))
			//.ForMember(P => P.PictureUrl, O => O.MapFrom(S => $"{"https://localhost:7295"}/{S.PictureUrl}"));
			//.ForMember(P => P.PictureUrl, O => O.MapFrom(S => $"{_configuration["ApiBaseUrl"]}/{S.PictureUrl}"));
			.ForMember(P => P.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());


			CreateMap<CustomerBasketDto, CustomerBasket>();
			CreateMap<BasketItemDto, BasketItem>();
		}
	}
}
