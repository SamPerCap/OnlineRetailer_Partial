using System.Collections.Generic;
using System.Linq;
using CustomersApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomersApi.Data
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly CustomerApiContext db;
        public CustomerRepository(CustomerApiContext context)
        {
            db = context;
        }
        Customer IRepository<Customer>.Add(Customer customer)
        {
            var newCustomer = db.Customers.Add(customer).Entity;
            db.SaveChanges();
            return newCustomer;
        }

        void IRepository<Customer>.Edit(Customer modifiedCustomer)
        {
            db.Entry(modifiedCustomer).State = EntityState.Modified;
            db.SaveChanges();
        }

        Customer IRepository<Customer>.Get(int id)
        {
            return db.Customers.FirstOrDefault(c => c.Id == id);
        }

        IEnumerable<Customer> IRepository<Customer>.GetAll()
        {
            return db.Customers.ToList();
        }

        void IRepository<Customer>.Remove(int id)
        {
            var customer = db.Customers.FirstOrDefault(c => c.Id == id);
            db.Customers.Remove(customer);
            db.SaveChanges();
        }
    }
}
