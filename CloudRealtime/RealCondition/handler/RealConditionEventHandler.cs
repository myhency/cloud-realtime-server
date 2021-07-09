using CloudRealtime.RealCondition.model;
using CloudRealtime.util;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudRealtime.RealCondition.handler
{
    public partial class RealConditionEventHandler
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private Opt10001EventHandler opt10001EventHandler;
        private MyTelegramBot myTelegramBot;
        private List<Condition> conditionList;
        private BindingList<string> stockItemList;
        string[] itemCodeList = { };

        public RealConditionEventHandler(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.opt10001EventHandler = new Opt10001EventHandler(axKHOpenAPI);
            this.myTelegramBot = new MyTelegramBot();
            this.stockItemList = new BindingList<string>();
            initialize();
        }

        private void initialize()
        {
            axKHOpenAPI1.OnReceiveConditionVer += axKHOpenAPI1_OnReceiveConditionVer;
            axKHOpenAPI1.OnReceiveTrCondition += axKHOpenAPI1_OnReceiveTrCondition;
            axKHOpenAPI1.OnReceiveRealCondition += axKHOpenAPI1_OnReceiveRealCondition;
            axKHOpenAPI1.OnReceiveTrData += axKHOpenAPI1_OnReceiveTrData;
            stockItemList.ListChanged += stockItemList_ListChanged;

            /**
             * 사용자 조건검색식의 실행순서:
             *  1. GetConditionLoad() -> 사용자의 조건식으로 로딩
             *  2. axKHOpenAPI1_OnReceiveConditionVer -> 사용자의 조건식리스트를 검색
             *  3. SendCondition -> 특정 조건식으로 종목검색
             *  4.1. axKHOpenAPI1_OnReceiveTrCondition -> 정적 검색 결과
             *  4.2. axKHOpenAPI1_OnReceiveRealCondition -> 실시간 검색 결과
             */
            this.axKHOpenAPI1.GetConditionLoad(); //사용자 조건검색식 로딩

            this.conditionList = new List<Condition>(); //사용자 조건식 리스트

            // TODO.test 후에 주석 풀기
            Thread t1 = new Thread(new ThreadStart(() =>
            {
                Logger.Info("유통거래량 전송 쓰레드 시작");
                TimeSpan triggerTime = new TimeSpan(16, 0, 0);
                while (true)
                {
                    TimeSpan timeNow = DateTime.Now.TimeOfDay;

                    if (timeNow > triggerTime)
                    {
                        sendCondition("3000", "유통거래량", false);
                        break;
                    }
                }
            }));

            t1.Start();

            Thread t2 = new Thread(new ThreadStart(() =>
            {
                Logger.Info("유통거래량 수집 쓰레드 시작");
                TimeSpan triggerTime = new TimeSpan(16, 0, 0);
                while (true)
                {
                    if (itemCodeList.Length > 0)
                    {
                        foreach (string itemCode in itemCodeList)
                        {
                            if (itemCode.Length > 0)
                            {
                                opt10001EventHandler.requestTrOpt10001(itemCode, itemCodeList.Length.ToString());
                                Thread.Sleep(1500);
                                itemCodeList = itemCodeList.Where(v => v != itemCode).ToArray();
                            }
                        }

                        Thread.Sleep(60000);
                        DateTime today = DateTime.Now;
                        DateTime startMarketTime = new DateTime(today.Year, today.Month, today.Day, 09, 0, 0);
                        string strNow = today.ToString("yyyy년 MM월 dd일");
                        string strDay = today.ToString("yyyy-MM-dd");
                        this.myTelegramBot.sendTextMessageAsyncToBot(
                            $"✔️ {strNow} 유통주식대비 거래량 비율 업데이트가 완료되었습니다." +
                            $"오늘 하루도 수고많으셨습니다. \n" +
                            $"http://221.140.88.147:13000/analyze/volume/" + strDay);
                        break;
                    }
                }
            }));

            t2.Start();

        }

        private void stockItemList_ListChanged(object sender, ListChangedEventArgs e)
        {
            opt10001EventHandler.requestTrOpt10001(stockItemList.ElementAt(e.NewIndex), "유통");
        }

        private void axKHOpenAPI1_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            Logger.Debug("axKHOpenAPI1_OnReceiveTrData");
        }

        private void axKHOpenAPI1_OnReceiveRealCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            Logger.Debug("axKHOpenAPI1_OnReceiveRealCondition");
        }

        private void axKHOpenAPI1_OnReceiveTrCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            Logger.Debug("axKHOpenAPI1_OnReceiveTrCondition");
            Logger.Debug(e.strCodeList);
            //string[] itemCodeList = e.strCodeList.TrimEnd(';').Split(';');
            this.itemCodeList = e.strCodeList.TrimEnd(';').Split(';');
            //List<string> itemList = new List<string>();
            //foreach (string itemCode in itemCodeList)
            //{
            //    if (itemCode.Length > 0)
            //    {
            //        this.stockItemList.Add(itemCode);
            //        //itemList.Add(itemCode);
            //        //this.opt10001EventHandler.requestTrOpt10001(itemCode, $"종목저장TR요청_{e.strConditionName}");
            //        //this.axKHOpenAPI1.SetInputValue("종목코드", itemCode);
            //        //int x = this.axKHOpenAPI1.CommRqData($"주식기본정보요청_{e.strConditionName}_{itemCode}", "opt10001", 0, "3000");
            //        //Logger.Info($"CommRqData result: {x}");
            //    }

            //    Thread.Sleep(1500);
            //    //break;
            //}

            //this.opt10001EventHandler = new Opt10001EventHandler(this.axKHOpenAPI1, itemList);

            //Thread.Sleep(10000);

            //this.opt10001EventHandler.sendFileAsyncToBot();
        }

        private void axKHOpenAPI1_OnReceiveConditionVer(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            Logger.Debug("axKHOpenAPI1_OnReceiveConditionVer");
            string conditionList = axKHOpenAPI1.GetConditionNameList();

            Logger.Info("사용자 조건식 로딩 완료");

            string[] conditionArray = conditionList.TrimEnd(';').Split(';');

            foreach (string conditionInfo in conditionArray)
            {
                if (conditionInfo.Length > 0)
                {
                    //condition[0] : 조건식 인덱스
                    //condition[1] : 조건식 이름
                    string[] condition = conditionInfo.Split('^');

                    this.conditionList.Add(new Condition(int.Parse(condition[0]), condition[1]));
                }
            }
        }

        public void sendCondition(string screenNumber, string conditionName, bool isRealTime)
        {
            Logger.Debug("sendCondition");
            foreach (Condition item in this.conditionList)
            {
                if (item.name.Equals(conditionName))
                {
                    int result = axKHOpenAPI1.SendCondition(
                        screenNumber,
                        item.name,
                        item.index,
                        isRealTime ? 1 : 0
                    );
                    Logger.Info("sendcondition result: " + result);
                    break;
                }
            }
        }
    }
}
