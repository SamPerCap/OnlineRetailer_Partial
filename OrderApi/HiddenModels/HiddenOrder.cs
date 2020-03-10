using System;
using System.Collections.Generic;

namespace OrderApi.Models
{
    public class HiddenOrder
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? customerId { get; set; }
        public OrderStatus Status { get; set; }
        public IList<HiddenOrderLine> OrderLines { get; set; }

        public enum OrderStatus
        {
            cancelled,
            completed,
            shipped,
            paid
        }

    }
}
