using GeneralStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GeneralStoreAPI.Controllers
{
    public class ProductController : ApiController
    {
        private readonly GeneralStoreDbContext _context = new GeneralStoreDbContext();
        //POST (create)
        //api/
        [HttpPost]
        public async Task<IHttpActionResult> CreateProduct([FromBody] Product model)
        {
            if (model is null)
            {
                return BadRequest("Your request body cannot be empty. ");
            }
            if (ModelState.IsValid)
            {
                //Store the model in the database
                _context.Product.Add(model);
                int changeCount = await _context.SaveChangesAsync();

                return Ok("Product created! ");
            }
            //The model is not valid, reject it
            return BadRequest(ModelState);
        }

        //Get ALL
        //api/Product
        [HttpGet]
        public async Task<IHttpActionResult> GetAllProducts()
        {
            List<Product> product = await _context.Product.ToListAsync();
            return Ok(product);
        }

        //Get by ID
        //api/Product{ID}
        [HttpGet]
        public async Task<IHttpActionResult> GetProductBySKU([FromUri] string sku)
        {
            Product product = await _context.Product.FindAsync(sku);

            if (product != null)
            {
                return Ok(product);
            }
            return NotFound();

        }

        //PUT (update)
        //api/Product/{id}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateProduct([FromUri] string sku, [FromBody] Product updatedProduct)
        {
            //check the ids if they match
            if (sku  != updatedProduct.SKU)
            {
                return BadRequest("Product SKUs do not match. ");
            }
            //Check the ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //find the product in the database
            Product product = await _context.Product.FindAsync(sku);
            //if product doesnt exist do something
            if (product is null)
                return NotFound();
            //Update the properties
            product.Name = product.Name;
            product.Cost = product.Cost;

            //Save the changes
            await _context.SaveChangesAsync();

            return Ok("The product was updated! ");
        }

        //DELETE (delete)
        //api/Product/{ID}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProduct([FromUri] string sku)
        {
            Product product = await _context.Product.FindAsync(sku);

            if (product is null)
                return NotFound();

            _context.Product.Remove(product);

            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The product was deleted. ");
            }
            return InternalServerError();
        }
    }
}
