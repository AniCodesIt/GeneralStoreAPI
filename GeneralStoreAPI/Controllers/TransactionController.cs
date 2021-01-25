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
    public class TransactionController : ApiController
    {
        private readonly GeneralStoreDbContext _context = new GeneralStoreDbContext();
        public ProductController _productWindow = new ProductController();
        //POST (create)
        //api/
        [HttpPost]
        //public async Task<IHttpActionResult> CreateTransaction([FromBody] Transaction model)
        public async Task<IHttpActionResult> CreateTransaction([FromBody] int YourCustomerId, string TheSKUyouWant, int HowManyYouWant)
        {
            //if (model is null)
            if(YourCustomerId < 1 || TheSKUyouWant is null || HowManyYouWant < 1)
            {
                return BadRequest("Your request body cannot be empty. ");
            }
            if (ModelState.IsValid)
            {
                Product catToy = (Product)await _productWindow.GetProductBySKU(TheSKUyouWant);
                if (catToy.NumberInInventory >= HowManyYouWant)
                {

                    Transaction MyTransaction = new Transaction();

                    MyTransaction.CustomerID = YourCustomerId;
                    MyTransaction.ProductSKU = TheSKUyouWant;
                    MyTransaction.ItemCount = HowManyYouWant;
                    MyTransaction.DateOfTransaction = DateTime.Today;
                   
                //Store the model in the database
                _context.Transaction.Add(MyTransaction);
                int changeCount = await _context.SaveChangesAsync();

                return Ok("Transaction created! ");
                }
                else
                {
                    return BadRequest("Not enough inventory. ");
                }

            }
            //Else if model is not valid, reject it
            else
            {
            return BadRequest(ModelState);
            }
        }

        //Get ALL
        //api/Transaction
        [HttpGet]
        public async Task<IHttpActionResult> GetAllTransactions()
        {
            List<Transaction> transaction = await _context.Transaction.ToListAsync();
            return Ok(transaction);
        }

        //Get by ID
        //api/Transaction{ID}
        [HttpGet]
        public async Task<IHttpActionResult> GetTransactionByID([FromUri] int id)
        {
            Transaction transaction = await _context.Transaction.FindAsync(id);

            if (transaction != null)
            {
                return Ok(transaction);
            }
            return NotFound();

        }

        //PUT (update)
        //api/Transaction/{id}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateTransaction([FromUri] int id, [FromBody] Transaction updatedTransaction)
        {
            //check the ids if they match
            if (id != updatedTransaction.ID)
            {
                return BadRequest("Transaction IDs do not match. ");
            }
            //Check the ModelState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //find the transaction in the database
            Transaction transaction = await _context.Transaction.FindAsync(id);
            //if transaction doesnt exist do something
            if (transaction is null)
                return NotFound();
            //Update the properties
            transaction.ItemCount = transaction.ItemCount;          

            //Save the changes
            await _context.SaveChangesAsync();

            return Ok("The transaction was updated! ");
        }

        //DELETE (delete)
        //api/Transaction/{ID}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteTransaction([FromUri] string id)
        {
            Transaction transaction = await _context.Transaction.FindAsync(id);

            if (transaction is null)
                return NotFound();

            _context.Transaction.Remove(transaction);

            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The transaction was deleted. ");
            }
            return InternalServerError();
        }
    }
}