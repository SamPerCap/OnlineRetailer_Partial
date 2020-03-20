﻿using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Data;
using SharedModels;
using System;
using System.Threading;

namespace ProductApi.Infastructure
{
    public class MessageListener
    {
        IServiceProvider provider;
        string connectionString;

        // The service provider is passed as a parameter, because the class needs
        // access to the product repository. With the service provider, we can create
        // a service scope that can provide an instance of the product repository.
        public MessageListener(IServiceProvider provider, string connectionString)
        {
            this.provider = provider;
            this.connectionString = connectionString;
        }

        public void Start()
        {
            using (var bus = RabbitHutch.CreateBus(connectionString))
            {
                // already implemented
                bus.Subscribe<OrderStatusChangedMessage>("productApiCompleted",
                    HandleOrderCompleted, x => x.WithTopic("completed"));

                // delete
                bus.Subscribe<OrderStatusChangedMessage>("productCancelled",
                    HandleOrderCancelled, x => x.WithTopic("cancelled"));

                // edit
                bus.Subscribe<OrderStatusChangedMessage>("productApiCompleted",
                    HandleOrderCompleted, x => x.WithTopic("shipped"));

                // create
                bus.Subscribe<OrderStatusChangedMessage>("productApiCompleted",
                    HandleOrderCompleted, x => x.WithTopic("paid"));

                bus.Respond<SharedProductAvailableRequest, SharedProductAvailableResponse>(request => CheckProductAvailable(request));

                // Add code to subscribe to other OrderStatusChanged events:
                // * cancelled
                // * shipped
                // * paid
                // Implement an event handler for each of these events.
                // Be careful that each subscribe has a unique subscription id
                // (this is the first parameter to the Subscribe method). If they
                // get the same subscription id, they will listen on the same
                // queue.

                // Block the thread so that it will not exit and stop subscribing.
                lock (this)
                {
                    Monitor.Wait(this);
                }
            }
        }

        private void HandleOrderCancelled(OrderStatusChangedMessage message)
        {
            // A service scope is created to get an instance of the product repository.
            // When the service scope is disposed, the product repository instance will
            // also be disposed.
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var productRepos = services.GetService<IRepository<SharedProducts>>();

                // Reserve items of ordered product (should be a single transaction).
                // Beware that this operation is not idempotent.
                foreach (var orderLine in message.SharedOrderLine)
                {
                    var product = productRepos.Get(orderLine.ProductId);
                    product.ItemsReserved += orderLine.Quantity;
                    productRepos.Remove(product.Id);
                }
            }
        }

        private SharedProductAvailableResponse CheckProductAvailable(SharedProductAvailableRequest request)
        {
            // A service scope is created to get an instance of the product repository.
            // When the service scope is disposed, the product repository instance will
            // also be disposed.
            using (var scope = provider.CreateScope())
            {
                var response = new SharedProductAvailableResponse();
                var services = scope.ServiceProvider;
                var productRepos = services.GetService<IRepository<SharedProducts>>();

                var product = productRepos.Get(request.ProductId);
                if (product.ItemsInStock >= request.Quantity && product.Name != null)
                {
                    response.ProductIsAvailable = true;
                }
                else
                {
                    response.ProductIsAvailable = false;
                }
                return response;
            }
        }

        private void HandleOrderCompleted(OrderStatusChangedMessage message)
        {
            // A service scope is created to get an instance of the product repository.
            // When the service scope is disposed, the product repository instance will
            // also be disposed.
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var productRepos = services.GetService<IRepository<SharedProducts>>();

                // Reserve items of ordered product (should be a single transaction).
                // Beware that this operation is not idempotent.
                foreach (var orderLine in message.SharedOrderLine)
                {
                    var product = productRepos.Get(orderLine.ProductId);
                    product.ItemsReserved += orderLine.Quantity;
                    productRepos.Edit(product);
                }
            }
        }
    }
}
