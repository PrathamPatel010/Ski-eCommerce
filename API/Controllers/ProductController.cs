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
        public async Task<ActionResult<PaginatedResult<Product>>> GetProducts(string? brands,string? types,string? sort,string? search,int pageIndex=0,int pageSize=3)
        {
            return Ok(await repo.GetProductsAsync(brands,types,sort,search,pageIndex,pageSize));
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

        [HttpGet("brands")]
        public async Task<ActionResult<List<string>>> GetBrands(){
            var brands = await repo.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<string>>> GetTypes(){
            var types = await repo.GetTypesAsync();
            return Ok(types);
        }
    }
}