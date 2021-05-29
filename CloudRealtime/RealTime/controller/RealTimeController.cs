using CloudRealtime.RealTime.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.RealTime.controller
{
    public partial class RealTimeController : IRealDataEventHandler
    {
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private AlarmService alarmService;

        public RealTimeController(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.alarmService = new AlarmService();
            this.alarmService.getAlarmList();
        }

        public void setRealReg(string screenNumber, string itemCode, string fidList, string type)
        {
            
        }
    }
}
