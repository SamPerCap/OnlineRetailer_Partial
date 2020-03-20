using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Data;
using OrderApi.Infastructure;
using OrderApi.Models;
using RestSharp;
using SharedModels;

namespace OrderApi.Controllers
{
    [Route("api/Orders")]
    public class OrdersController : Controller
    {
        private readonly IRepository<SharedOrders> repository;
        private readonly IServiceGateway<SharedProducts> productGateway;
        private readonly IServiceGateway<SharedCustomers> customerGateway;
        private readonly IMessagePublisher messagePublisher;

        public OrdersController(IRepository<SharedOrders> repos,
            IServiceGateway<SharedProducts> gateway,
            IMessagePublisher publisher)
        {
            repository = repos;
            productGateway = gateway;
            messagePublisher = publisher;
        }

        // GET: api/orders
        [HttpGet]
        public IEnumerable<SharedOrders> Get()
        {
            return repository.GetAll();
        }

        // GET api/products/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/orders
        [HttpPost]
        public IActionResult Post([FromBody]SharedOrders order)
        {
            if (order == null || order.customerId < 1)
            {
                return BadRequest();
            }
            var cust = CustomerExists(order);
            if (cust == true && cust != null)
            {
                try
                {
                    foreach (var item in order.OrderLines)
                    {
                        if (!messagePublisher.ProductExists(item.ProductId, item.Quantity))
                        {
                            return StatusCode(500, "Not enough items in stock.");
                        }
                    }

                    // Publish OrderStatusChangedMessage. If this operation
                    // fails, the order will not be created
                    messagePublisher.PublishOrderStatusChangedMessage(
                        order.customerId, order.OrderLines, "completed");

                    // Create order.
                    order.Status = SharedOrders.OrderStatus.completed;
                    var newOrder = repository.Add(order);
                    return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);
                }
                catch (Exception e)
                {
                    return StatusCode(500, "An error happened. Try again." + e);
                }
            }
            else
            {
                // If there are not enough product items available.
                return StatusCode(500, "The user is not registered");
            }
        }

        private bool? CustomerExists(SharedOrders orders)
        {
            return messagePublisher.PublishCustomerExists(orders.customerId);
        }

    }
}
