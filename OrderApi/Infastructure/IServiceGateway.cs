using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Infastructure
{
    public interface IServiceGateway<T>
    {
        T Get(int id);
    }
}
