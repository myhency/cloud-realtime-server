using CloudRealtime.RealCondition.handler;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.RealCondition.controller
{
    public partial class RealConditionController
    {
        private RealConditionEventHandler realConditionEventHandler;

        public RealConditionController(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.realConditionEventHandler = new RealConditionEventHandler(axKHOpenAPI);
        }
    }
}
