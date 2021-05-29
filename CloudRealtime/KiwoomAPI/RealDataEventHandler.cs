using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudRealtime.model;

namespace CloudRealtime
{
    public partial class RealDataEventHandler
    {
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private ITrader iTrader;
        bool isMarketOpen = false;

        public RealDataEventHandler(ITrader iTrader, AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.iTrader = iTrader;
            axKHOpenAPI1.OnReceiveRealData += axKHOpenAPI1_OnReceiveRealData;
        }

        private void axKHOpenAPI1_OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            Console.WriteLine(e.sRealType);
            DateTime today = DateTime.Now;
            DateTime startMarketTime = new DateTime(today.Year, today.Month, today.Day, 09, 0, 0);
            DateTime volumeTriggerTime = new DateTime(today.Year, today.Month, today.Day, 09, 3, 0);
            string strNow = today.ToString("yyyy-MM-dd HH:mm:ss");

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
                    Console.WriteLine("장이 시작되었습니다.");
                }
            }

            //장시작되면 데이터수집
            if (e.sRealType.Equals("주식체결"))
            {
                string itemCode = e.sRealKey.Trim();
                //체결시간을 String 이 아닌 DateTime 형태로 저장하기 위함
                string executionTime = axKHOpenAPI1.GetCommRealData(itemCode, 20).Trim();
                DateTime eTime;
                executionTime = DateTime.Now.ToString("yyyyMMdd") + executionTime;
                eTime = DateTime.ParseExact(executionTime, "yyyyMMddHHmmss", null);
                int presentPrice = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(itemCode, 10))); //현재가
                int.Parse(axKHOpenAPI1.GetCommRealData(itemCode, 11)); //전일대비
                double.Parse(axKHOpenAPI1.GetCommRealData(itemCode, 12)); //등락율
                Math.Abs(long.Parse(axKHOpenAPI1.GetCommRealData(itemCode, 15))); //거래량
                long.Parse(axKHOpenAPI1.GetCommRealData(itemCode, 13)); //누적거래량
                long.Parse(axKHOpenAPI1.GetCommRealData(itemCode, 14)); //누적거래대금
                Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(itemCode, 16))); //시가
                Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(itemCode, 17))); //고가
                Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(itemCode, 18))); //저가
                double.Parse(axKHOpenAPI1.GetCommRealData(itemCode, 30)); //전일거래량대비
                string itemName = axKHOpenAPI1.GetMasterCodeName(itemCode).Trim(); //종목명

                //Console.WriteLine($"{executionTime} {itemCode} {itemName} {presentPrice}");

                RealtimeStockSigningData realtimeStockSigningData = new RealtimeStockSigningData(
                   itemCode,
                   executionTime,
                   axKHOpenAPI1.GetCommRealData(itemCode, 10).Trim(),
                   axKHOpenAPI1.GetCommRealData(itemCode, 11).Trim(),
                   axKHOpenAPI1.GetCommRealData(itemCode, 12).Trim(),
                   axKHOpenAPI1.GetCommRealData(itemCode, 15).Trim(),
                   axKHOpenAPI1.GetCommRealData(itemCode, 13).Trim(),
                   axKHOpenAPI1.GetCommRealData(itemCode, 14).Trim(),
                   axKHOpenAPI1.GetCommRealData(itemCode, 16).Trim(),
                   axKHOpenAPI1.GetCommRealData(itemCode, 17).Trim(),
                   axKHOpenAPI1.GetCommRealData(itemCode, 18).Trim(),
                   axKHOpenAPI1.GetCommRealData(itemCode, 30).Trim(),
                   axKHOpenAPI1.GetMasterCodeName(itemCode).Trim()
                );

                //이 시점부터 거래/알람이 시작된다
                iTrader.setRealtimeStockSigningData(realtimeStockSigningData);
            }
            else if(e.sRealType.Equals("잔고"))
            {
                string itemCode = e.sRealKey.Trim();
                Console.WriteLine(axKHOpenAPI1.GetCommRealData(itemCode, 9201));
            }
        }

        public void setRealReg(string screenNumber, string itemCode, string fidList, string type)
        {
            axKHOpenAPI1.SetRealReg(screenNumber, itemCode, fidList, type);
        }
    }
}
