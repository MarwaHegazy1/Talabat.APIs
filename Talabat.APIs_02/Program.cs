
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Text;
using Talabat.APIs_02.Errors;
using Talabat.APIs_02.Extensions;
using Talabat.APIs_02.Helpers;
using Talabat.APIs_02.Middlewares;
using Talabat.Application.AuthService;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Infrastructure;
using Talabat.Infrastructure.Data;
using Talabat.Infrastructure.Identity;

namespace Talabat.APIs_02
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the container.

			#region SwaggerServicesExtension

			builder.Services.AddControllers();
			//.AddNewtonsoftJson(options =>
			//{
			//	options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			//});

			#endregion

			builder.Services.AddSwaggerServices();

			builder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
			});

			#region DI BasketRepo
			builder.Services.AddScoped<IConnectionMultiplexer>((serviceProvider) =>
			{
				var connection = builder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});
			#endregion

		

			#region Security 
			builder.Services.AddAuthServices(builder.Configuration);

			#endregion

			#endregion

			#region ApplicationServicesExtension

			builder.Services.AddApplicationServices();

			#endregion

			var app = builder.Build();

			#region Update-Database & DataSeeding

			using var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var _dbContext = services.GetRequiredService<StoreContext>();
			var _IdentityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();

			// ASK CLR for Creating Object from DbContext Explicitly

			var loggerFactory = services.GetRequiredService<ILoggerFactory>();

			try
			{
				await _dbContext.Database.MigrateAsync(); // Update-Database
				await StoreContextSeed.SeedAsyunc(_dbContext); // DataSeeding
				await _IdentityDbContext.Database.MigrateAsync();


				var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				await ApplicationIdentityDataSeed.SeedUsersAsync(_userManager);
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "an error has been occured during apply the migration");
			}
			#endregion


			#region Configure KestrelMiddlewares
			app.UseMiddleware<ExceptionMiddleware>();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				#region SwaggerServicesExtension3

				app.UseSwaggerMiddlewares();
				#endregion
			}

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.UseStaticFiles();

			app.MapControllers();
			#endregion

			app.Run();
		}
	}
}
