using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.APIs_02.Errors;
using Talabat.APIs_02.Helpers;
using Talabat.Application.AuthService;
using Talabat.Application.OrderService;
using Talabat.Application.PaymentService;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Infrastructure;

namespace Talabat.APIs_02.Extensions
{
	public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
			#region DI OrderServies
			services.AddScoped(typeof(IOrderService), typeof(OrderService));
			#endregion

			#region DI UnitOfWork
			services.AddScoped(typeof(IOrderServies), typeof(UnitOfWork));
			#endregion
		
			#region DI Basket
			services.AddScoped(typeof(IBasketRepository), typeof(BasketRespository)); 
			#endregion

			#region DI
			//services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
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


		public static IServiceCollection AddAuthServices(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer("Bearer", options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuer = true,
						ValidIssuer = configuration["JWT:ValidIssuer"],
						ValidateAudience = true,
						ValidAudience = configuration["JWT:ValidAudience"],
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Authkey"] ?? string.Empty)),
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero,
					};
				});
			#region DI IAuthService
			services.AddScoped(typeof(IAuthService), typeof(AuthService));
			#endregion

			return services;
		}
	}
}
