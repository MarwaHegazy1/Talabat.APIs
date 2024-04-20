﻿using System.Net;
using System.Text.Json;
using Talabat.APIs_02.Errors;

namespace Talabat.APIs_02.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IWebHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> loggerFactory, IWebHostEnvironment env)
		{
			_next = next;
			_logger = loggerFactory;
			_env = env;
		}
		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next.Invoke(httpContext);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);

				httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				httpContext.Response.ContentType = "application/json";

				var response = _env.IsDevelopment() ?
					new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
					: new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

				var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
				var json = JsonSerializer.Serialize(response, options);
				await httpContext.Response.WriteAsync(json);
			}

		}
	}
}
