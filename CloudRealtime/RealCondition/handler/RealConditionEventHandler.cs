using CloudRealtime.RealCondition.model;
using NLog;
using System;
using System.Collections.Generic;
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
        private List<Condition> conditionList;

        public RealConditionEventHandler(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.opt10001EventHandler = new Opt10001EventHandler(axKHOpenAPI);
            initialize();
        }

        private void initialize()
        {
            axKHOpenAPI1.OnReceiveConditionVer += axKHOpenAPI1_OnReceiveConditionVer;
            axKHOpenAPI1.OnReceiveTrCondition += axKHOpenAPI1_OnReceiveTrCondition;
            axKHOpenAPI1.OnReceiveRealCondition += axKHOpenAPI1_OnReceiveRealCondition;

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

            
        }

        private void axKHOpenAPI1_OnReceiveRealCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            Logger.Debug("axKHOpenAPI1_OnReceiveRealCondition");
        }

        private void axKHOpenAPI1_OnReceiveTrCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            Logger.Debug("axKHOpenAPI1_OnReceiveTrCondition");
            Logger.Debug(e.strCodeList);
            string[] itemCodeList = e.strCodeList.TrimEnd(';').Split(';');
            foreach (string itemCode in itemCodeList)
            {
                if (itemCode.Length > 0)
                {
                    this.opt10001EventHandler.requestTrOpt10001(itemCode, $"종목저장TR요청_{e.strConditionName}");
                }

                Thread.Sleep(1500);
                //break;
            }

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

            //TODO.test 후에 주석 풀기
            //Thread t1 = new Thread(new ThreadStart(() =>
            //{
            //    Logger.Info("유통거래량 쓰레드 시작");
            //    TimeSpan triggerTime = new TimeSpan(15, 40, 0);
            //    while (true)
            //    {
            //        TimeSpan timeNow = DateTime.Now.TimeOfDay;

            //        if (timeNow > triggerTime)
            //        {
            //            //Thread.Sleep(180000);
            //            sendCondition("3000","유통거래량_코스피", true);
            //            break;
            //        }
            //    }
            //}));

            //t1.Start();

            //Thread t2 = new Thread(new ThreadStart(() =>
            //{
            //    Logger.Info("유통거래량 쓰레드 시작");
            //    TimeSpan triggerTime = new TimeSpan(15, 40, 0);
            //    while (true)
            //    {
            //        TimeSpan timeNow = DateTime.Now.TimeOfDay;

            //        if (timeNow > triggerTime)
            //        {
            //            sendCondition("3001", "유통거래량_코스닥", true);
            //            break;
            //        }
            //    }
            //}));

            //t2.Start();

            sendCondition("3000", "유통거래량_코스피", false);
            //sendCondition("3001","유통거래량_코스닥", false);
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
