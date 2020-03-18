using EasyNetQ;
using SharedModels;
using System;
using System.Collections.Generic;

namespace OrderApi.Infastructure
{
    public class MessagePublisher : IMessagePublisher, IDisposable
    {
        IBus bus;
        public MessagePublisher(string connectionString)
        {
            bus = RabbitHutch.CreateBus(connectionString);
        }

        public void Dispose()
        {
            bus.Dispose();
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
        public void PublishCustomerExists(int customerId, string topic)
        {
            var message = new SharedCustomers
            {
                Id = customerId
            };
            bus.Publish(message, topic);
        }
        public void PublishSharedProducts(int Id, string topic)
        {
            var message = new SharedProducts
            {
                Id = Id
            };

            bus.Publish(message, topic);
        }
    }
}
