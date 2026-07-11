using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http.HttpResults;
using API.RequestHelper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IGenericRepository<Product> _repo) : ControllerBase
    {
       
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);
            var Products = await _repo.ListAsync(spec);
            var Count=await _repo.CountAsync(spec);
            var pagination = new Pagination<Product>(specParams.PageIndex, specParams.PageSize, Count, Products);
            return Ok (pagination);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repo.GetByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            var brands = await _repo.ListAsync(spec);
            return Ok(brands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            var types = await _repo.ListAsync(spec);
            return Ok(types);
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
      
        private bool ProductExists(int id)
        {
            return _repo.Exists(id);
        }
    }
}
