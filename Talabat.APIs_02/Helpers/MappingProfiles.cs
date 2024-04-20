﻿using AutoMapper;
using Talabat.APIs_02.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs_02.Helpers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Product, ProductToReturnDto>()
				.ForMember(P => P.Brand, O => O.MapFrom(S => S.Brand.Name))
				.ForMember(P => P.Category, O => O.MapFrom(S => S.Category.Name));
				//.ForMember(P => P.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

		}
	}
}