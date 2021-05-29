using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudRealtime.model;

namespace CloudRealtime
{
    public interface ITrader
    {
        void setRealtimeStockSigningData(RealtimeStockSigningData realtimeStockSigningData);
    }
}
