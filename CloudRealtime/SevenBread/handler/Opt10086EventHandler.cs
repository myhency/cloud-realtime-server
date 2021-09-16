using CloudRealtime.model;
using CloudRealtime.SevenBread.service;
using CloudRealtime.StockItem.service;
using CloudRealtime.util;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudRealtime.SevenBread.handler
{
    public partial class Opt10086EventHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private StockItemService stockItemService;
        private SevenBreadService sevenBreadService;
        private OtherFunctions otherFunctions;
        private MyTelegramBot myTelegramBot;
        private int screenNumber = 7002;

        public Opt10086EventHandler(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.myTelegramBot = new MyTelegramBot();
            this.stockItemService = new StockItemService();
            this.sevenBreadService = new SevenBreadService();
            this.otherFunctions = new OtherFunctions(axKHOpenAPI);
            this.axKHOpenAPI1.OnReceiveTrData += axKHOpenAPI1_OnReceiveTrData;

            initialize();
        }

        private void initialize()
        {
        }

        public void requestTrOpt10086(string itemCode, string detectedDate, string trName)
        {
            logger.Debug($"requestTrOpt10086: {itemCode}, {detectedDate}, trName: {trName}");
            this.axKHOpenAPI1.SetInputValue("종목코드", itemCode);
            this.axKHOpenAPI1.SetInputValue("조회일자", detectedDate);
            this.axKHOpenAPI1.SetInputValue("표시구분", "0");
            int x = this.axKHOpenAPI1.CommRqData($"주식기본정보요청_{trName}_{itemCode}", "opt10086", 0, screenNumber.ToString());
            logger.Debug($"requestTrOpt10086 result : {x}");
        }

        private void axKHOpenAPI1_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            logger.Debug("axKHOpenAPI1_OnReceiveTrData");

        }

        private Opt10086VO getOpt10086VO(string sTrCode, string sRQName)
        {
            Opt10086VO opt10086VO = new Opt10086VO();
            opt10086VO.종가 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "종가").Trim()));
            opt10086VO.거래량 = int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "거래량").Trim());
            opt10086VO.등락률 = float.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "등락율").Trim());
            opt10086VO.전일비 = int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "전일비").Trim());
            
            return opt10086VO;
        }
    }
}
