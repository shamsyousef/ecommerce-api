using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IGenericRepository<Product> _repo) : ControllerBase
    {
       
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand , string? type )
        {
            var spec = new ProductSpecification(brand, type);
            var Products = await _repo.ListAsync(spec);
            return Ok (Products);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repo.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _repo.Add(product);
            
            if (await _repo.SaveAllAsync())
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            
            return BadRequest("Problem creating product");
        }
        [HttpPost("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !ProductExists(id))
                return BadRequest("Product ID mismatch");
            _repo.Update(product);
            if (await _repo.SaveAllAsync())
                return NoContent();
            return BadRequest("Problem updating product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            _repo.Remove(product);
            if (await _repo.SaveAllAsync())
                return NoContent();
            return BadRequest("Problem deleting product");
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok();
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok();
        }
        private bool ProductExists(int id)
        {
            return _repo.Exists(id);
        }
    }
}
