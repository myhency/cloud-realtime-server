using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using CloudRealtime.SevenBread.service;
using CloudRealtime.util;
using CloudRealtime.model;

namespace CloudRealtime.SevenBread.handler
{
    public partial class Opt10081EventHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private SevenBreadService sevenBreadService;
        private OtherFunctions otherFunctions;
        private MyTelegramBot myTelegramBot;
        private int screenNumber = 7002;

        public Opt10081EventHandler(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.myTelegramBot = new MyTelegramBot();
            this.sevenBreadService = new SevenBreadService();
            this.otherFunctions = new OtherFunctions(axKHOpenAPI);
            this.axKHOpenAPI1.OnReceiveTrData += axKHOpenAPI1_OnReceiveTrData;

            initialize();
        }

        private void initialize()
        {
        }

        public void requestTrOpt10081(string itemCode, string capturedDate, string sujungJuga, string trName)
        {
            logger.Debug($"requestTrOpt10081: {itemCode}, trName: {trName}");
            DateTime today = DateTime.Now;
            string strDay = today.ToString("yyyy-MM-dd");

            this.axKHOpenAPI1.SetInputValue("종목코드", itemCode);
            this.axKHOpenAPI1.SetInputValue("기준일자", strDay.Replace("-", ""));
            this.axKHOpenAPI1.SetInputValue("수정주가구분", sujungJuga);
            int x = this.axKHOpenAPI1.CommRqData($"주식기본정보요청_{trName}_{itemCode}_{capturedDate.Replace("-", "")}", "opt10081", 0, screenNumber.ToString());
            logger.Debug($"requestTrOpt10081 result : {x}");
        }

        private void axKHOpenAPI1_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            if (e.sRQName.Contains("주식기본정보요청_007빵_일봉차트"))
            {
                List<Opt10081VO> opt10081VOList = getOpt10081VO(e.sTrCode, e.sRQName);
                string capturedDate = e.sRQName.Split('_')[4];
                string itemCode = e.sRQName.Split('_')[3];

                foreach (Opt10081VO item in opt10081VOList)
                {
                    item.종목코드 = itemCode;
                    string result = this.sevenBreadService.updateSevenBreadChartToday(item);
                    logger.Info(result, itemCode, item.일자);
                    //if (result == "Conflict") break;
                    if (item.일자 == capturedDate) break;
                    Thread.Sleep(300);
                }
            }
        }

        private List<Opt10081VO> getOpt10081VO(string sTrCode, string sRQName)
        {
            List<Opt10081VO> opt10081VOList = new List<Opt10081VO>();
            for (int i = 0; i < 90; i++)
            {
                Opt10081VO opt10081VO = new Opt10081VO
                {
                    종목코드 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, i, "종목코드").Trim(),
                    일자 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, i, "일자").Trim(),
                    고가 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, i, "고가").Trim(),
                    저가 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, i, "저가").Trim(),
                    현재가 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, i, "현재가").Trim(),
                    수정주가구분 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, i, "수정주가구분").Trim(),
                    수정비율 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, i, "수정비율").Trim(),
                    수정주가이벤트 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, i, "수정주가이벤트").Trim(),
                };

                opt10081VOList.Add(opt10081VO);

            }

            return opt10081VOList;
        }
    }
}
