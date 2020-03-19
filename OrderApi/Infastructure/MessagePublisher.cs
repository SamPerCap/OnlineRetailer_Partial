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
        int id;
        public MessagePublisher(string connectionString)
        {
            bus = RabbitHutch.CreateBus(connectionString);
        }

        public void Dispose()
        {
            bus.Dispose();
        }

        public SharedProducts ProductExists(SharedProducts prod)
        {
            Thread.Sleep(TimeSpan.FromSeconds(5));
            bus.Receive<SharedProducts>("productIsAvailable", message => MethodForProduct(message));
            prod.Id = id;
            return prod;
        }

        private void MethodForProduct(SharedProducts message)
        {
            id = message.Id;
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

        public void PublishSharedProducts(int Id,  string topic)
        {
            var message = new SharedProducts
            {
                Id = Id
            };

            bus.Publish(message, topic);
        }
    }
}
