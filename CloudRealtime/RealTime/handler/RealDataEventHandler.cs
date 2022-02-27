using CloudRealtime.RealTime.controller;
using CloudRealtime.RealTime.model;
using CloudRealtime.RealTime.service;
using CloudRealtime.util;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.RealTime.handler
{
    public partial class RealDataEventHandler
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private AlarmService alarmService;
        private IFirebaseConfig config;
        private IFirebaseClient client;
        private MyTelegramBot myTelegramBot;

        private List<Alarm> alarmList;
        bool isMarketOpen = false;

        public RealDataEventHandler(
            AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI,
            List<Alarm> alarmList
        )
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.alarmService = new AlarmService();
            this.alarmList = alarmList;
            this.myTelegramBot = new MyTelegramBot();

            initialize();
        }

        private void initialize()
        {
            axKHOpenAPI1.OnReceiveRealData += axKHOpenAPI1_OnReceiveRealData;
            config = new FirebaseConfig
            {
                AuthSecret = "mYXKscnYNaaHOfLp6Lgyo6xpZBQT1ZoA5N1iv5Vl",
                BasePath = "https://bread-stock.firebaseio.com/"
            };

            client = new FireSharp.FirebaseClient(config);

            if (client != null)
            {
                Logger.Info("Connected to firebase");
            }
        }

        public void setAlarmList(List<Alarm> alarmList)
        {
            this.alarmList = alarmList;
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

            //Console.WriteLine(e.sRealType);

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

            if (e.sRealType.Equals("주식체결") && alarmList.Exists(v => v.itemCode == e.sRealKey) && isMarketOpen)
            {
                int presentPrice = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(e.sRealKey, 10))); //현재가
                double fluctuationRate = double.Parse(axKHOpenAPI1.GetCommRealData(e.sRealKey, 12)); //등락율

                Alarm alarm = this.alarmList.FirstOrDefault(v => v.itemCode.Equals(e.sRealKey));

                if(presentPrice >= alarm.recommendPrice 
                    && (alarm.alarmStatus.Equals("ALARM_CREATED") 
                    || alarm.alarmStatus.Equals("PRICE_UPDATED"))) //돌파가격보다 같거나 큰 경우
                {
                    Logger.Info($"{alarm.itemName} 종목 알림상태: {alarm.alarmStatus}");
                    //COMPLETE. alarmList에서 해당종목을 제거하지 않는다. 손절가격이 올 경우 알람을 줘야 한다.
                    //대신 알람의 상태를 변경하고 다시 alarmList에 넣어야 한다.
                    alarm.alarmStatus = "ALARMED";

                    Logger.Info($"{alarm.itemName} 종목 알림상태 변경됨 ==> {alarm.alarmStatus}");

                    //COMPLETE. 알람을 전송한다.
                    //COMPLETE. markdown 형태로 전송할때 오류가 발생하여 메세지 포맷을 변경함(7/14)
                    string message = $"📈 가격돌파 알림 \n" +
                        $"\n" +
                        $"종목명 : {alarm.itemName} \n" +
                        $"기준가격 {String.Format("{0:#,###}", alarm.recommendPrice)}원을 돌파했습니다. \n" +
                        $"손절가격 {String.Format("{0:#,###}", alarm.losscutPrice)}원을 설정하세요. \n" +
                        $"현재가 : {String.Format("{0:#,###}", presentPrice)} ({fluctuationRate}%)\n" +
                        $"\n" +
                        $"{alarm.comment} \n" +
                        $"\n" +
                        $"{alarm.theme} \n";

                    Logger.Info(message);

                    // TODO. try/catch 로 감쌀 것
                    myTelegramBot.sendTextMessageAsyncToSwingBot(message);

                    //알리미 서버에 알람상태를 업데이트 한다.
                    alarmService.buyAlarm(alarm.alarmId);
                }

                if (presentPrice <= alarm.losscutPrice 
                    && (alarm.alarmStatus.Equals("ALARMED")
                    || alarm.alarmStatus.Equals("PRICE_UPDATED"))) //손절가격보다 작거나 같은경우
                {
                    Logger.Info($"{alarm.itemName} 종목 알림상태: {alarm.alarmStatus}");
                    //COMPLETE. alarmList에서 해당종목을 제거한다.
                    alarmList.Remove(alarm);
                    Logger.Info($"{alarm.itemName} 종목 삭제됨");

                    //COMPLETE. 알람을 전송한다.
                    //COMPLETE. markdown 형태로 전송할때 오류가 발생하여 메세지 포맷을 변경함(7/14)
                    string message = $"📉 가격이탈 알림 \n" +
                        $"\n" +
                        $"종목명 : {alarm.itemName} \n" +
                        $"기준가격 {String.Format("{0:#,###}", alarm.losscutPrice)}원을 이탈했습니다. \n" +
                        $"현재가 : {String.Format("{0:#,###}", presentPrice)} ({fluctuationRate}%)\n" +
                        $"\n" +
                        $"{alarm.comment} \n" +
                        $"\n" +
                        $"{alarm.theme} \n";

                    Logger.Info(message);

                    // TODO. try/catch 로 감쌀 것
<<<<<<< HEAD
                    myTelegramBot.sendTextMessageAsyncToSwingBot(message);
=======
                    // myTelegramBot.sendTextMessageAsyncToBot(message);
>>>>>>> server-change

                    //알리미 서버에 알람상태를 업데이트 한다.
                    alarmService.losscutAlarm(alarm.alarmId);
                }
            }
        }
    }
}
