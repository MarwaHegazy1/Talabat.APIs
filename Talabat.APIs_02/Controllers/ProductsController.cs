using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs_02.Dtos;
using Talabat.APIs_02.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecifications;

namespace Talabat.APIs_02.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productsRepo;
		private readonly IMapper _mapper;
		private readonly IGenericRepository<ProductBrand> _brandsRepo;
		private readonly IGenericRepository<ProductCategory> _categoriesRepo;

		public ProductsController(IGenericRepository<Product> productsRepo
			,IMapper mapper,
			IGenericRepository<ProductBrand> brandsRepo,
			IGenericRepository<ProductCategory> categoriesRepo
			)
		{
			_productsRepo = productsRepo;
			_mapper = mapper;
			_brandsRepo = brandsRepo;
			_categoriesRepo = categoriesRepo;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts()
		{
			var spec = new ProductWithBrandAndCategorySpecifications();
			var products = await _productsRepo.GetAllWithSpecAsync(spec);

			return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products));
		}

		[ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(id);
			var product = await _productsRepo.GetWithSpecAsync(spec);

			if (product is null)
				return NotFound(new ApiResponse(404));

			return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
		}

		[HttpGet("brands")]
		public async Task<ActionResult<IEnumerable<ProductBrand>>> GetBrands()
		{
			var brands = await _brandsRepo.GetAllAsync();
			return Ok(brands);
		}
		[HttpGet("categories")]
		public async Task<ActionResult<IEnumerable<ProductCategory>>> GetCategories()
		{
			var categories = await _categoriesRepo.GetAllAsync();
			return Ok(categories);
		}
	}
}
