using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ProductApi.Models;

namespace ProductApi.Data
{
    public class ProductRepository : IRepository<HiddenProduct>
    {
        private readonly ProductApiContext db;

        public ProductRepository(ProductApiContext context)
        {
            db = context;
        }

        HiddenProduct IRepository<HiddenProduct>.Add(HiddenProduct entity)
        {
            var newProduct = db.Products.Add(entity).Entity;
            db.SaveChanges();
            return newProduct;
        }

        void IRepository<HiddenProduct>.Edit(HiddenProduct entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        HiddenProduct IRepository<HiddenProduct>.Get(int id)
        {
            return db.Products.FirstOrDefault(p => p.Id == id);
        }

        IEnumerable<HiddenProduct> IRepository<HiddenProduct>.GetAll()
        {
            return db.Products.ToList();
        }

        void IRepository<HiddenProduct>.Remove(int id)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            db.Products.Remove(product);
            db.SaveChanges();
        }
    }
}
