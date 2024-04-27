using Microsoft.AspNetCore.Mvc;
using Talabat.APIs_02.Errors;
using Talabat.APIs_02.Helpers;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastructure;

namespace Talabat.APIs_02.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			#region DI Basket
			services.AddScoped(typeof(IBasketRepository), typeof(BasketRespository)); 
			#endregion

			#region DI
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			#endregion

			#region AutoMapper
			services.AddAutoMapper(typeof(MappingProfiles));
			#endregion

			#region Validation Error Handling
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
														 .SelectMany(P => P.Value.Errors)
														 .Select(E => E.ErrorMessage)
														 .ToList();

					var response = new ApiValidationErrorResponse()
					{
						Errors = errors
					};

					return new BadRequestObjectResult(response);
				};
			});
			#endregion 

			return services;
		}
	}
}
