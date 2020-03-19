﻿using EasyNetQ;
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
            var request = new SharedProductAvailableRequest()
            {
                ProductId = prodId,
                Quantity = amount
            };

            var response = bus.Request<SharedProductAvailableRequest, SharedProductAvailableResponse>(request);

            return response.ProductIsAvailable;
        }

        static void MethodForProduct(SharedProducts message)
        {

            if (message.Name != null)
            {
                name=message.Name;
                id =message.Id;
            }
            else
            {
                name = null;
                id = 0;
            }
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

            bus.Send(topic, message);
        }
    }
}
