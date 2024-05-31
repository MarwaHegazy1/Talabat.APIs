﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs_02.Helpers
{
	public class CachedAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int _timeToLiveInSeconds;

		public CachedAttribute(int timeToLiveInSeconds)
        {
			_timeToLiveInSeconds = timeToLiveInSeconds;
		}
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var responseCacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

			var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
			var response = await responseCacheService.GetCachedResponseAsyn(cacheKey);

			if (!string.IsNullOrEmpty(response))
			{
				var result = new ContentResult()
				{
					Content = response,
					ContentType = "application/json",
					StatusCode = 200,
				};
				context.Result = result;
				return;
			}
			
			var executedActionContext = await next.Invoke();

			if(executedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
			{
				await responseCacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
			}

		}

		private string GenerateCacheKeyFromRequest(HttpRequest request)
		{
			var KeyBuilder = new StringBuilder();
			KeyBuilder.Append(request.Path);
			foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
				KeyBuilder.Append($"|{key}-{value}");
				 
			return KeyBuilder.ToString();
        }
	}
}
