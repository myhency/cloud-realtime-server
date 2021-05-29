using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.KiwoomAPI
{
    public partial class OrderEventHandler
    {
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private ITrader iTrader;

        public OrderEventHandler(ITrader iTrader, AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.iTrader = iTrader;
        }
    }
}
