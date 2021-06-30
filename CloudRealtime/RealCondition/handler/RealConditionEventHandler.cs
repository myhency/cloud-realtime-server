using CloudRealtime.RealCondition.model;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    this.opt10001EventHandler.requestTrOpt10001(itemCode, $"관심종목저장TR요청_{e.strConditionName}");
                }

                //Thread.Sleep(3000);
            }
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

            sendCondition("유통거래량_코스피", false);
        }

        public void sendCondition(string conditionName, bool isRealTime)
        {
            Logger.Debug("sendCondition");
            foreach (Condition item in this.conditionList)
            {
                if (item.name.Equals(conditionName))
                {
                    axKHOpenAPI1.SendCondition(
                        "3000",
                        item.name,
                        item.index,
                        isRealTime ? 1 : 0
                    );

                    break;
                }
            }
        }
    }
}
