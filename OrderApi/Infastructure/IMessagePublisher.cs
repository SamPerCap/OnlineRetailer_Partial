﻿using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infastructure
{
    public interface IMessagePublisher
    {
        void PublishSharedProducts(int Id, string topic);
        void PublishCustomerExists(int customerId, string topic);
        void PublishOrderStatusChangedMessage(int? customerId,
        IList<SharedOrderLine> orderLines, string topic);
    }
}
