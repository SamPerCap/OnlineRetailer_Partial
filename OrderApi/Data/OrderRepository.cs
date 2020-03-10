using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OrderApi.Models;
using System;
using SharedModels;

namespace OrderApi.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderApiContext db;

        public OrderRepository(OrderApiContext context)
        {
            db = context;
        }

        public SharedOrders Add(SharedOrders entity)
        {
            if (entity.Date == null)
                entity.Date = DateTime.Now;

            var newOrder = db.Orders.Add(entity).Entity;
            db.SaveChanges();
            return newOrder;
        }

        public void Edit(SharedOrders entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public SharedOrders Get(int id)
        {
            return db.Orders.Include(o => o.OrderLines).FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<SharedOrders> GetAll()
        {
            return db.Orders.ToList();
        }

        public IEnumerable<SharedOrders> GetByCustomer(int customerId)
        {
            var ordersForCustomer = from o in db.Orders
                                    where o.customerId == customerId
                                    select o;

            return ordersForCustomer.ToList();
        }

        public void Remove(int id)
        {
            var order = db.Orders.FirstOrDefault(p => p.Id == id);
            db.Orders.Remove(order);
            db.SaveChanges();
        }
    }
}
