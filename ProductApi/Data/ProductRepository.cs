using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ProductApi.Models;
using SharedModels;

namespace ProductApi.Data
{
    public class ProductRepository : IRepository<SharedProducts>
    {
        private readonly ProductApiContext db;

        public ProductRepository(ProductApiContext context)
        {
            db = context;
        }

        SharedProducts IRepository<SharedProducts>.Add(SharedProducts entity)
        {
            var newProduct = db.Products.Add(entity).Entity;
            db.SaveChanges();
            return newProduct;
        }

        void IRepository<SharedProducts>.Edit(SharedProducts entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        SharedProducts IRepository<SharedProducts>.Get(int id)
        {
            return db.Products.FirstOrDefault(p => p.Id == id);
        }

        IEnumerable<SharedProducts> IRepository<SharedProducts>.GetAll()
        {
            return db.Products.ToList();
        }

        void IRepository<SharedProducts>.Remove(int id)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            db.Products.Remove(product);
            db.SaveChanges();
        }
    }
}
