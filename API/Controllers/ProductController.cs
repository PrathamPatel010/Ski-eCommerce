using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController(IProductRepository repo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            return Ok(await repo.GetProductsAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await repo.GetProductByIdAsync(id);
            
            if(product==null){
                return NotFound();
            }
            
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult>CreateProduct(Product product){
            repo.AddProduct(product);
            if(await repo.SaveChangesAsync())
            {
                return CreatedAtAction("GetProductById",new {id=product.Id},product);
            }
            return BadRequest("Error creating a product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id,Product product)
        {
            if(product.Id!=id || !ProductExists(id))
            {
                return BadRequest();
            }
            
            repo.UpdateProduct(product);
            if(await repo.SaveChangesAsync()){
                return NoContent();
            }
            
            return BadRequest("Error updating a product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id){
            var product = await repo.GetProductByIdAsync(id);
            if(product==null)   return NotFound();
            
            repo.DeleteProduct(product);        
            if(await repo.SaveChangesAsync()){
                return NoContent();
            }
            return BadRequest("Error updating a product"); 
        }

        private bool ProductExists(int id){
            return repo.ProductExist(id);
        }
    }
}