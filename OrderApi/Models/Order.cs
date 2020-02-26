using System;
namespace OrderApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
        public int Quantity { get; set; }
    }
}
