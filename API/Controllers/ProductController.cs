using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private StoreContext _context;
        public ProductController(StoreContext context){
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(){
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id){
            var product = await _context.Products.FindAsync(id);
            
            if(product==null){
                return NotFound();
            }
            
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>>CreateProduct(Product product){
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product; 
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id,Product product)
        {
            if(product.Id!=id || !ProductExists(id))
            {
                return BadRequest();
            }
            
            _context.Entry(product).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id){
            var product = await _context.Products.FindAsync(id);
            if(product==null){
                return NotFound();        
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ProductExists(int id){
            return _context.Products.Any(x=>x.Id==id);
        }
    }
}