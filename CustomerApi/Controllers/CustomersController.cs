using System.Collections.Generic;
using CustomersApi.Models;
using CustomersApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace CustomersApi.Controllers
{
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        private readonly IRepository<HiddenCustomer> repository;

        public CustomersController(IRepository<HiddenCustomer> repos)
        {
            repository = repos;
        }
        // GET: api/Customer
        [HttpGet]
        public IEnumerable<HiddenCustomer> Get()
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
        public IActionResult Post([FromBody] HiddenCustomer customer)
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
        public IActionResult Put(int id, [FromBody] HiddenCustomer customer)
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
