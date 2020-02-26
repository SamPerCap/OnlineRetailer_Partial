using System.Collections.Generic;
using CustomerApi.Models;
using CustomerApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApi.Controllers
{
    [Route("api/Customer")]
    public class CustomerController : Controller
    {
        private readonly IRepository<Customer> repository;

        public CustomerController(IRepository<Customer> repos)
        {
            repository = repos;
        }
        // GET: api/Customer
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return repository.GetAll();
        }

        // GET: api/Customer/5
        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST: api/Customer
        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }
            var newCustomer = repository.Add(customer);
            return CreatedAtRoute("GetCustomer", new
            {
                id = newCustomer.Id,
                name = newCustomer.Name
            },
                newCustomer);
        }

        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Customer customer)
        {
            if (customer == null || customer.Id != id)
            {
                return BadRequest();
            }

            var modifiedCustomer = repository.Get(id);

            if (modifiedCustomer == null)
            {
                return NotFound();
            }

            modifiedCustomer.Name = customer.Name;
            modifiedCustomer.Email = customer.Email;
            modifiedCustomer.Phone = customer.Phone;
            modifiedCustomer.BillingAddress = customer.BillingAddress;
            modifiedCustomer.ShippingAddress = customer.ShippingAddress;
            modifiedCustomer.CreditStanding = customer.CreditStanding;

            repository.Edit(modifiedCustomer);
            return new NoContentResult();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (repository.Get(id) == null)
            {
                return NotFound();
            }
            repository.Remove(id);
            return new NoContentResult();
        }
    }
}
