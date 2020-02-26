using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public List<Order> Order { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public int CreditStanding { get; set; }
    }
}
