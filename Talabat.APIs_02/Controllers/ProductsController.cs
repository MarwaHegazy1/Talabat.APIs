﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.APIs_02.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productsRepo;

		public ProductsController(IGenericRepository<Product> productsRepo)
		{
			_productsRepo = productsRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await _productsRepo.GetAllAsync();
			return Ok(products);
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await _productsRepo.GetAsync(id);

			if (product is null)
				return NotFound();

			return Ok(product);
		}
	}
}
