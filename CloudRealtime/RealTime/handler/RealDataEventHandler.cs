using CloudRealtime.RealTime.controller;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.RealTime.handler
{
    public partial class RealDataEventHandler : IRealDataEventHandler
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private IRealTimeController iRealTimeController;
        bool isMarketOpen = false;

        public RealDataEventHandler(IRealTimeController iRealTimeController, AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.iRealTimeController = iRealTimeController;
            axKHOpenAPI1.OnReceiveRealData += axKHOpenAPI1_OnReceiveRealData;
        }

        public void setRealReg(string screenNumber, string itemCode, string fidList, string type)
        {
            this.axKHOpenAPI1.SetRealReg(screenNumber, itemCode, fidList, type);
        }

        private void axKHOpenAPI1_OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            DateTime today = DateTime.Now;
            DateTime startMarketTime = new DateTime(today.Year, today.Month, today.Day, 09, 0, 0);
            string strNow = today.ToString("yyyy-MM-dd HH:mm:ss");

            Console.WriteLine(e.sRealType);

            //장시작시간 체크
            if (today > startMarketTime)
            {
                isMarketOpen = true;
            }

            if (e.sRealType.Equals("장시작시간"))
            {
                string value = axKHOpenAPI1.GetCommRealData(e.sRealKey, 215);
                if (value.Equals("3"))
                {
                    isMarketOpen = true;
                    Logger.Debug("장이 시작되었습니다.");
                }
            }

            if (e.sRealType.Equals("주식체결") && isMarketOpen)
            {

            }
        }
    }
}
