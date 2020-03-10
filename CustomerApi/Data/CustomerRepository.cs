using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace CustomersApi.Data
{
    public class CustomerRepository : IRepository<SharedCustomers>
    {
        private readonly CustomerApiContext db;
        public CustomerRepository(CustomerApiContext context)
        {
            db = context;
        }
        SharedCustomers IRepository<SharedCustomers>.Add(SharedCustomers customer)
        {
            var newCustomer = db.Customers.Add(customer).Entity;
            db.SaveChanges();
            return newCustomer;
        }

        void IRepository<SharedCustomers>.Edit(SharedCustomers modifiedCustomer)
        {
            db.Entry(modifiedCustomer).State = EntityState.Modified;
            db.SaveChanges();
        }

        SharedCustomers IRepository<SharedCustomers>.Get(int id)
        {
            return db.Customers.FirstOrDefault(c => c.Id == id);
        }

        IEnumerable<SharedCustomers> IRepository<SharedCustomers>.GetAll()
        {
            return db.Customers.ToList();
        }

        void IRepository<SharedCustomers>.Remove(int id)
        {
            var customer = db.Customers.FirstOrDefault(c => c.Id == id);
            db.Customers.Remove(customer);
            db.SaveChanges();
        }
    }
}
