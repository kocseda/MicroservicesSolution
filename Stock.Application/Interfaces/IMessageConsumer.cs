using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application.Interfaces
{
    public interface IMessageConsumer
    {
        void StartConsuming<T>() where T : class;
    }
}
