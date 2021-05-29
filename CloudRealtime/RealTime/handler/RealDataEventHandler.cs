using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.RealTime.handler
{
    public partial class RealDataEventHandler
    {
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;

        public RealDataEventHandler(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            axKHOpenAPI1.OnReceiveRealData += axKHOpenAPI1_OnReceiveRealData;
        }

        private void axKHOpenAPI1_OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {

        }
    }
}
