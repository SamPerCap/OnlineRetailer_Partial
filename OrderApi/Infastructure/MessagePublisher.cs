using EasyNetQ;
using OrderApi.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderApi.Infastructure
{
    public class MessagePublisher : IMessagePublisher, IDisposable
    {
        IBus bus;
        static int id;
        static string name;

        public MessagePublisher(string connectionString)
        {
            bus = RabbitHutch.CreateBus(connectionString);
        }

        public void Dispose()
        {
            bus.Dispose();
        }

        public bool ProductExists(int prodId, int amount)
        {
            var message = new SharedProductAvailableRequest()
            {
                ProductId = prodId,
                Quantity = amount
            };

            var response = bus.Request<SharedProductAvailableRequest, SharedProductAvailableResponse>(message);

            return response.ProductIsAvailable;
        }

        public bool CustomerExists(int custId)
        {
            var message = new SharedCustomerRequest()
            {
                CustomerId = custId
            };

            var response = bus.Request<SharedCustomerRequest, SharedCustomerResponse>(message);

            return response.Exists;
        }

        public void PublishOrderStatusChangedMessage(int? customerId, IList<SharedOrderLine> orderLines, string topic)
        {
            var message = new OrderStatusChangedMessage
            {
                CustomerId = customerId,
                SharedOrderLine = orderLines
            };

            bus.Publish(message, topic);
        }
        public bool PublishCustomerExists(int customerId)
        {
            var message = new SharedCustomerRequest
            {
                CustomerId = customerId
            };
            var response = bus.Request<SharedCustomerRequest, SharedCustomerResponse>(message);
            return response.Exists;
        }
        public void PublishSharedProducts(int Id, string topic)
        {
            var message = new SharedProducts
            {
                Id = Id
            };

            bus.Send(topic, message);
        }

        public void CancelOrder(int? customerId, IList<SharedOrderLine> orderLines, string topic)
        {
            var message = new OrderStatusChangedMessage
            {
                CustomerId = customerId,
                SharedOrderLine = orderLines
            };

            bus.Publish(message, topic);
        }
    }
}
