using System;
namespace OrderApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public Product ProductId { get; set; }
        public Customer Customer { get; set; }
        public int Quantity { get; set; }
    }
}
