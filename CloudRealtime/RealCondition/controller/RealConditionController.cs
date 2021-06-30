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
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private RealConditionEventHandler realConditionEventHandler;

        public RealConditionController(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.realConditionEventHandler = new RealConditionEventHandler(axKHOpenAPI);
            //this.realConditionEventHandler.sendCondition("유통거래량_코스피", false);
        }

        public void sendCondition(string conditionName, bool isRealTime)
        {
            this.realConditionEventHandler.sendCondition(conditionName, isRealTime);
        }
    }
}
