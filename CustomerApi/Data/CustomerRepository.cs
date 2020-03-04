using System.Collections.Generic;
using System.Linq;
using CustomersApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomersApi.Data
{
    public class CustomerRepository : IRepository<HiddenCustomer>
    {
        private readonly CustomerApiContext db;
        public CustomerRepository(CustomerApiContext context)
        {
            db = context;
        }
        HiddenCustomer IRepository<HiddenCustomer>.Add(HiddenCustomer customer)
        {
            var newCustomer = db.Customers.Add(customer).Entity;
            db.SaveChanges();
            return newCustomer;
        }

        void IRepository<HiddenCustomer>.Edit(HiddenCustomer modifiedCustomer)
        {
            db.Entry(modifiedCustomer).State = EntityState.Modified;
            db.SaveChanges();
        }

        HiddenCustomer IRepository<HiddenCustomer>.Get(int id)
        {
            return db.Customers.FirstOrDefault(c => c.Id == id);
        }

        IEnumerable<HiddenCustomer> IRepository<HiddenCustomer>.GetAll()
        {
            return db.Customers.ToList();
        }

        void IRepository<HiddenCustomer>.Remove(int id)
        {
            var customer = db.Customers.FirstOrDefault(c => c.Id == id);
            db.Customers.Remove(customer);
            db.SaveChanges();
        }
    }
}
