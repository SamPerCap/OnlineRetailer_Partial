using ProductApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Infastructure
{
    public class ProductServiceGateway : IServiceGateway<HiddenProduct>
    {
        public HiddenProduct Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}
