﻿using System;
namespace OrderApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Order Order { get; set; }
        public decimal Price { get; set; }
        public int ItemsInStock { get; set; }
        public int ItemsReserved { get; set; }
    }
}
