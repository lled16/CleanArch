using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchMvc.API.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categories = await _productService.GetProducts();

            if (categories == null)
            {
                return NotFound("Products not found !");
            }
            return Ok(categories);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var category = await _productService.GetById(id);

            if (category == null)
            {
                return NotFound("Product not found !");
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDTO productDTO)
        {
            if (productDTO is null)
            {
                return BadRequest("Invalid body");
            }
            await _productService.Add(productDTO);

            return new CreatedAtRouteResult("GetProduct", new { id = productDTO.Id });
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] ProductDTO productDTO)
        {
            if (id != productDTO.Id || productDTO is null)
            {
                return BadRequest();
            }
            await _productService.Update(productDTO);

            return Ok(productDTO);

        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var product = _productService.GetById(id);

            if (product is null)
            {
                return NotFound("Product not found !");
            }
            await _productService.Remove(id);
            return Ok(product);
        }
    }
}

