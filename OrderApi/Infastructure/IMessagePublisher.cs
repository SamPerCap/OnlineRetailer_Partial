using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Infastructure
{
    public interface IMessagePublisher
    {
        void PublishOrderStatusChangedMessage(int customerId,
        IList<SharedOrderLine> orderLines, string topic);
    }
}
