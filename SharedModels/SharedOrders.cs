using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels
{
    public class SharedOrders
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int customerId { get; set; }
        public OrderStatus Status { get; set; }
        public IList<SharedOrderLine> OrderLines { get; set; }

        public enum OrderStatus
        {
            cancelled,
            completed,
            shipped,
            paid
        }
    }

    public class SharedOrderLine
    {
        public int id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
