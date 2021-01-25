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
    public class CustomerController : ApiController
    {

            private readonly GeneralStoreDbContext _context = new GeneralStoreDbContext();
            //POST (create)
            //api/
            [HttpPost]
            public async Task<IHttpActionResult> CreateCustomer([FromBody] Customer model)
            {
                if (model is null)
                {
                    return BadRequest("Your request body cannot be empty. ");
                }
                if (ModelState.IsValid)
                {
                    //Store the model in the database
                    _context.Customer.Add(model);
                    int changeCount = await _context.SaveChangesAsync();

                    return Ok("Customer created! ");
                }
                //The model is not valid, reject it
                return BadRequest(ModelState);
            }

            //Get ALL
            //api/
            [HttpGet]
            public async Task<IHttpActionResult> GetAllCustomers()
            {
                List<Customer> customer = await _context.Customer.ToListAsync();
                return Ok(customer);
            }

            //Get by ID
            //api/
            [HttpGet]
            public async Task<IHttpActionResult> GetCustomerByID([FromUri] int id)
            {
                Customer customer = await _context.Customer.FindAsync(id);

                if (customer != null)
                {
                    return Ok(customer);
                }
                return NotFound();



            }

            //PUT (update)
            //api/Customer/{id}
            [HttpPut]
            public async Task<IHttpActionResult> UpdateCustomer([FromUri] int id, [FromBody] Customer updatedCustomer)
            {
                //check the ids if they match
                if (id != updatedCustomer.ID)
                {
                    return BadRequest("Customer IDs do not match. ");
                }
                //Check the ModelState
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                //find the customer in the database
                Customer customer = await _context.Customer.FindAsync(id);
                //if restaurant doesnt exist do something
                if (customer is null)
                    return NotFound();
                //Update the properties
                customer.FirstName = updatedCustomer.FirstName;
                customer.LastName = updatedCustomer.LastName;

                //Save the changes
                await _context.SaveChangesAsync();

                return Ok("The customer name was updated! ");
            }

            //DELETE (delete)
            //api/Customer/{ID}
            [HttpDelete]
            public async Task<IHttpActionResult> DeleteCustomer([FromUri] int id)
            {
                Customer customer = await _context.Customer.FindAsync(id);

                if (customer is null)
                    return NotFound();

                _context.Customer.Remove(customer);

                if (await _context.SaveChangesAsync() == 1)
                {
                    return Ok("The customer was deleted. ");
                }
                return InternalServerError();
            }
        }
    }
}
