using EasyNetQ;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Threading;

namespace OrderApi.Infastructure
{
    public class MessagePublisher : IMessagePublisher, IDisposable
    {
        IBus bus;
        static int timeoutInterval = 1000;
        private bool orderExists = false;
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

        public void PublishSharedProducts(int Id,  string topic)
        {
            var message = new SharedProducts
            {
                Id = Id
            };

            bus.Publish(message, topic);
            Timer timer = new Timer(Timeout_Elapsed, message, timeoutInterval, Timeout.Infinite);
        }

        private void Timeout_Elapsed(object state)
        {
            bus.Receive<SharedProducts>("productIsAvailable", message => HandleOrderRequest(message));
        }

        private void HandleOrderRequest(SharedProducts message)
        {
            if (message != null)
            {
                orderExists = true;
            }
            else
            {
                orderExists = false;
            }
        }

        public bool DoesOrderExist()
        {
            return orderExists;
        }
    }
}
