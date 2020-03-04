﻿using System;
namespace OrderApi.Models
{
    public class HiddenOrder
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; }

    }
    public enum HiddenOrderStatus
    {
        Completed,
        Canceled,
        Shipped,
        Paid
    }
}
