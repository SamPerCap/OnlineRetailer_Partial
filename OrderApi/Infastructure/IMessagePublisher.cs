﻿using OrderApi.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infastructure
{
    public interface IMessagePublisher
    {
        void PublishSharedProducts(int Id, string topic);
        bool PublishCustomerExists(int customerId);
        void PublishOrderStatusChangedMessage(int? customerId,
        IList<SharedOrderLine> orderLines, string topic);
        bool ProductExists(int ProductId, int Amount);
        bool CustomerExists(int CustId);
    }
}
