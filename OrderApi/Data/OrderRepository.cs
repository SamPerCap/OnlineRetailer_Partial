using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OrderApi.Models;
using System;

namespace OrderApi.Data
{
    public class OrderRepository : IRepository<HiddenOrder>
    {
        private readonly OrderApiContext db;

        public OrderRepository(OrderApiContext context)
        {
            db = context;
        }

        HiddenOrder IRepository<HiddenOrder>.Add(HiddenOrder entity)
        {
            if (entity.Date == null)
                entity.Date = DateTime.Now;
            
            var newOrder = db.Orders.Add(entity).Entity;
            db.SaveChanges();
            return newOrder;
        }

        void IRepository<HiddenOrder>.Edit(HiddenOrder entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        HiddenOrder IRepository<HiddenOrder>.Get(int id)
        {
            return db.Orders.FirstOrDefault(o => o.Id == id);
        }

        IEnumerable<HiddenOrder> IRepository<HiddenOrder>.GetAll()
        {
            return db.Orders.ToList();
        }

        void IRepository<HiddenOrder>.Remove(int id)
        {
            var order = db.Orders.FirstOrDefault(p => p.Id == id);
            db.Orders.Remove(order);
            db.SaveChanges();
        }
    }
}
