﻿using CloudRealtime.SevenBread.model;
using CloudRealtime.util;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudRealtime.SevenBread.handler
{
    public partial class RealDataEventHandler
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private IFirebaseConfig config;
        private IFirebaseClient client;
        private MyTelegramBot myTelegramBot;

        private List<SevenBreadItem> sevenBreadItemList = new List<SevenBreadItem>();
        private List<SevenBreadItem> realTimeSevenBreadItemList = new List<SevenBreadItem>();

        bool isMarketOpen = false;

        public RealDataEventHandler(
            AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI,
            List<SevenBreadItem> sevenBreadItemList
        )
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.myTelegramBot = new MyTelegramBot();
            //Note. 여기서 sevenBreadItemList 를 deep copy 해주어야 함.
            //      그렇지 않으면 알람을 주고 list 에서 종목을 삭제할 때
            //      RealTimeController 에서 System.InvalidOperationException 이 발생함.
            //      그래서 선언도 sevenBreadItemList = new List<SevenBreadItem>(); 이렇게 해 줌.
            try
            {
                if (sevenBreadItemList == null) goto point;
                foreach (SevenBreadItem item in sevenBreadItemList)
                {
                    this.sevenBreadItemList.Add(item);
                }
            }
            catch (Exception e)
            {
                Logger.Error($"[get sevenBread list]{e.Message}");
            }

            point:;
            
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

        public void setSevenBreadItemList(List<SevenBreadItem> sevenBreadItemList)
        {
            this.sevenBreadItemList = sevenBreadItemList;
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

            //TODO. 007빵 리스트에서 가져온 종목 처리할 로직 구현하기
            if (e.sRealType.Equals("주식체결") && sevenBreadItemList.Exists(v => v.itemCode == e.sRealKey) && isMarketOpen)
            {
                SevenBreadItem sevenBreadItem = this.sevenBreadItemList.FirstOrDefault(v => v.itemCode.Equals(e.sRealKey));

                int presentPrice = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(e.sRealKey, 10))); //현재가
                double fluctuationRate = double.Parse(axKHOpenAPI1.GetCommRealData(e.sRealKey, 12)); //등락율
                double fluctuationRateBy = 100 - Math.Round(double.Parse(sevenBreadItem.capturedPrice.ToString()) / double.Parse(presentPrice.ToString()) * 100, 2);

                //어제 종가가 기준가보다 낮은 종목이 현재가가 기준가보다 올라갈 때 알림을 준다
                //매수알림
                if (presentPrice > sevenBreadItem.capturedPrice 
                    && sevenBreadItem.closingPrice < sevenBreadItem.capturedPrice)
                {
                    Logger.Info($"{sevenBreadItem.itemName} 종목 기준가 돌파 알림");
                    this.sevenBreadItemList.Remove(sevenBreadItem);
                    sevenBreadItem.alarmStatus = "UP";
                    sevenBreadItem.presentPrice = presentPrice;
                    sevenBreadItem.fluctuationRate = fluctuationRate;
                    sevenBreadItem.fluctuationRateBy = fluctuationRateBy;

                    this.realTimeSevenBreadItemList.Add(sevenBreadItem);

                    string message = $"📈 007빵 종목 기준가격 돌파 알림 \n" +
                        $"\n" +
                        $"종목명 : {sevenBreadItem.itemName} \n" +
                        $"기준가격 {String.Format("{0:#,###}", sevenBreadItem.capturedPrice)}원을 돌파했습니다. \n" +
                        $"현재가 : {String.Format("{0:#,###}", presentPrice)} ({fluctuationRate}%)\n" +
                        $"편입일 : {sevenBreadItem.capturedDate} \n" +
                        $"\n" +
                        $"{sevenBreadItem.theme} \n" +
                        $"\n" +
                        $"https://m.alphasquare.co.kr/service/chart?code=" + sevenBreadItem.itemCode;

                    Logger.Info(message);

                    myTelegramBot.sendTextMessageAsyncToSwingBot(message);

                    insertIntoFireBase(sevenBreadItem);

                    
                }
                //어제 종가가 기준가보다 높은 종목이 현재가가 기준가보다 내려갈 때 알림을 준다
                //손절알림
                else if (presentPrice < sevenBreadItem.capturedPrice
                    && sevenBreadItem.closingPrice > sevenBreadItem.capturedPrice)
                {
                    Logger.Info($"{sevenBreadItem.itemName} 종목 기준가 이탈 알림");
                    this.sevenBreadItemList.Remove(sevenBreadItem);

                    sevenBreadItem.alarmStatus = "DOWN";

                    string message = $"📉 007빵 종목 기준가격 이탈 알림 \n" +
                        $"\n" +
                        $"종목명 : {sevenBreadItem.itemName} \n" +
                        $"기준가격 {String.Format("{0:#,###}", sevenBreadItem.capturedPrice)}원을 이탈했습니다. \n" +
                        $"현재가 : {String.Format("{0:#,###}", presentPrice)} ({fluctuationRate}%)\n" +
                        $"편입일 : {sevenBreadItem.capturedDate} \n" +
                        $"\n" +
                        $"{sevenBreadItem.theme} \n" +
                        $"\n" +
                        $"https://m.alphasquare.co.kr/service/chart?code=" + sevenBreadItem.itemCode;

                    Logger.Info(message);

                    myTelegramBot.sendTextMessageAsyncToSwingBot(message);

                    insertIntoFireBase(sevenBreadItem);
                }

                
            }
            //TODO. 007빵 알림이 나간 종목들에 대해 실시간 가격 감시 로직
            else if(e.sRealType.Equals("주식체결") && realTimeSevenBreadItemList.Exists(v => v.itemCode == e.sRealKey) && isMarketOpen)
            {
                SevenBreadItem realTimeSevenBreadItem = this.realTimeSevenBreadItemList.FirstOrDefault(v => v.itemCode.Equals(e.sRealKey));

                int presentPrice = Math.Abs(int.Parse(axKHOpenAPI1.GetCommRealData(e.sRealKey, 10))); //현재가
                double fluctuationRate = double.Parse(axKHOpenAPI1.GetCommRealData(e.sRealKey, 12)); //등락율
                double fluctuationRateBy = 100 - Math.Round(double.Parse(realTimeSevenBreadItem.capturedPrice.ToString()) / double.Parse(presentPrice.ToString()) * 100, 2);

                realTimeSevenBreadItem.presentPrice = presentPrice;
                realTimeSevenBreadItem.fluctuationRateBy = fluctuationRateBy;

                updateToFireBase(realTimeSevenBreadItem);

                Logger.Debug($"[007빵] 알림나간 종목 실시간 가격 변동: " +
                    $"{realTimeSevenBreadItem.itemName}(기준가: {realTimeSevenBreadItem.capturedPrice}) " +
                    $"=> 현재가: {presentPrice}, " +
                    $"기준가대비 상승률: {(100 - Math.Round(double.Parse(realTimeSevenBreadItem.capturedPrice.ToString()) / double.Parse(presentPrice.ToString())) * 100, 2)}%");
            }
        }

        private void insertIntoFireBase(SevenBreadItem sevenBreadItem)
        {
            string itemCode = sevenBreadItem.itemCode;
            string itemName = sevenBreadItem.itemName;
            DateTime today = DateTime.Now;
            string strNow = today.ToString("HH:mm:ss");
            string strToday = today.ToString("yyyyMMdd");
            string path = $"sevenbread/alarm/{strToday}/{itemCode}";

            var data = new SevenBreadItem
            {
                itemCode = itemCode,
                itemName = itemName,
                closingPrice = sevenBreadItem.closingPrice,
                capturedPrice = sevenBreadItem.capturedPrice,
                capturedDate = sevenBreadItem.capturedDate,
                fluctuationRate = sevenBreadItem.fluctuationRate,
                alarmedTime = strNow,
                alarmStatus = sevenBreadItem.alarmStatus,
                presentPrice = sevenBreadItem.presentPrice,
                fluctuationRateBy = sevenBreadItem.fluctuationRateBy,
            };

            insertDataToFirebase(path, data);
        }

        private void updateToFireBase(SevenBreadItem sevenBreadItem)
        {
            string itemCode = sevenBreadItem.itemCode;
            string itemName = sevenBreadItem.itemName;
            DateTime today = DateTime.Now;
            string strNow = today.ToString("HH:mm:ss");
            string strToday = today.ToString("yyyyMMdd");
            string path = $"sevenbread/alarm/{strToday}/{itemCode}";

            var data = new SevenBreadItem
            {
                itemCode = itemCode,
                itemName = itemName,
                closingPrice = sevenBreadItem.presentPrice,
                capturedPrice = sevenBreadItem.capturedPrice,
                capturedDate = sevenBreadItem.capturedDate,
                fluctuationRate = sevenBreadItem.fluctuationRate,
                alarmedTime = strNow,
                alarmStatus = sevenBreadItem.alarmStatus,
                presentPrice = sevenBreadItem.presentPrice,
                fluctuationRateBy = sevenBreadItem.fluctuationRateBy,
            };

            updateDataToFirebase(path, data);
        }

        public async void insertDataToFirebase(string path, SevenBreadItem data)
        {
            SetResponse response = await client.SetAsync(path, data);
            Logger.Debug($"[007빵][firebase] 알림데이터 등록 완료:{response.ResultAs<SevenBreadItem>().itemName}");
        }

        public async Task<SevenBreadItem> selectDataFromFirebase(string path)
        {
            FirebaseResponse response = await client.GetAsync(path);
            return response.ResultAs<SevenBreadItem>();
        }

        public async void updateDataToFirebase(string path, SevenBreadItem data)
        {
            FirebaseResponse response = await client.UpdateAsync(path, data);
            Logger.Debug($"[firebase] Success to update data:{response.ResultAs<SevenBreadItem>().itemName}");
        }

        public async void deleteDataFromFirebase(string itemCode)
        {
            DateTime today = DateTime.Now;
            string strToday = today.ToString("yyyyMMdd");
            string path = $"sevenbread/alarm/{strToday}/{itemCode}";
            FirebaseResponse response = await client.DeleteAsync(path);
            Logger.Debug($"[firebase] Success to delete data:{path} --> {response.StatusCode}");
        }
    }
}
