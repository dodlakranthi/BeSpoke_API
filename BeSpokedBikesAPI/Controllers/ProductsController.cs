using BeSpokedBikesAPI.Interfaces;
using BeSpokedBikesAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeSpokedBikesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IBeSpokeBikesRepo _repo;
        public ProductsController(IBeSpokeBikesRepo repo)
        {
            _repo = repo;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var response = await _repo.GetAllProducts();
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, UpdateProductRequest product)
        {
            if (product == null || id != product.ProductId)
            {
                return BadRequest();
            }
           
            var response = await _repo.UpdateProduct(product);
            if(response != null && response.IsSuccess) 
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
