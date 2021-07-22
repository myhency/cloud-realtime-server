using CloudRealtime.RealCondition.service;
using CloudRealtime.RealTime.service;
using CloudRealtime.util;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudRealtime.RealTime.handler
{
    public partial class Opt10001EventHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private StockItemService stockItemService;
        private SevenBreadService sevenBreadService;
        private OtherFunctions otherFunctions;
        private MyTelegramBot myTelegramBot;
        private int screenNumber = 7001;
        private string path;

        public Opt10001EventHandler(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
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

        public void requestTrOpt10001(string itemCode, string trName)
        {
            logger.Debug($"requestTrOpt10001: {itemCode}, trName: {trName}");
            this.axKHOpenAPI1.SetInputValue("종목코드", itemCode);
            int x = this.axKHOpenAPI1.CommRqData($"주식기본정보요청_{trName}_{itemCode}", "opt10001", 0, screenNumber.ToString());
            logger.Debug($"requestTrOpt10001 result : {x}");
        }

        private void axKHOpenAPI1_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            logger.Debug("axKHOpenAPI1_OnReceiveTrData");
            if (e.sRQName.Contains("주식기본정보요청_007빵_"))
            {
                Opt10001VO opt10001VO = getOpt10001VO(e.sTrCode, e.sRQName);
                logger.Debug(e.sTrCode);
                logger.Debug(e.sRQName);

                sevenBreadService.updateSevenBreadItemCapturedDay(opt10001VO);
                
            }
            else if (e.sRQName.Contains("주식기본정보요청_007빵종가업데이트_"))
            {
                Opt10001VO opt10001VO = getOpt10001VO(e.sTrCode, e.sRQName);
                logger.Debug(e.sTrCode);
                logger.Debug(e.sRQName);
                sevenBreadService.updateSevenBreadItemToday(opt10001VO);
            }
        }

        public void sendFileAsyncToBot()
        {
            myTelegramBot.sendFileAsyncToBot(this.path);
        }

        private Opt10001VO getOpt10001VO(string sTrCode, string sRQName)
        {
            Opt10001VO opt10001VO = new Opt10001VO();
            opt10001VO.종목코드 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "종목코드").Trim();
            opt10001VO.종목명 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "종목명").Trim();
            opt10001VO.결산월 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "결산월").Trim();
            opt10001VO.액면가 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "액면가").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "액면가").Trim());
            opt10001VO.자본금 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "자본금").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "자본금").Trim());
            opt10001VO.상장주식 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "상장주식").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "상장주식").Trim());
            opt10001VO.신용비율 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "신용비율").Trim()) ? 0 : float.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "신용비율").Trim());
            opt10001VO.연중최고 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "연중최고").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "연중최고").Trim());
            opt10001VO.연중최저 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "연중최저").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "연중최저").Trim());
            opt10001VO.시가총액 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "시가총액").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "시가총액").Trim());
            opt10001VO.시가총액비중 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "시가총액비중").Trim();
            opt10001VO.외인소진률 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "외인소진률").Trim()) ? 0 : float.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "외인소진률").Trim());
            opt10001VO.대용가 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "대용가").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "대용가").Trim());
            opt10001VO.PER = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "PER").Trim()) ? 0 : float.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "PER").Trim());
            opt10001VO.EPS = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "EPS").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "EPS").Trim());
            opt10001VO.ROE = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "ROE").Trim()) ? 0 : float.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "ROE").Trim());
            opt10001VO.PBR = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "PBR").Trim()) ? 0 : float.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "PBR").Trim());
            opt10001VO.EV = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "EV").Trim()) ? 0 : float.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "EV").Trim());
            opt10001VO.BPS = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "BPS").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "BPS").Trim());
            opt10001VO.매출액 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "매출액").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "매출액").Trim());
            opt10001VO.영업이익 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "영업이익").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "영업이익").Trim());
            opt10001VO.당기순이익 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "당기순이익").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "당기순이익").Trim());
            opt10001VO.최고250 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "250최고").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "250최고").Trim());
            opt10001VO.최저250 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "250최저").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "250최저").Trim());
            opt10001VO.시가 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "시가").Trim()));
            opt10001VO.고가 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "고가").Trim()));
            opt10001VO.저가 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "저가").Trim()));
            opt10001VO.상한가 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "상한가").Trim()));
            opt10001VO.하한가 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "하한가").Trim()));
            opt10001VO.기준가 = int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "기준가").Trim());
            opt10001VO.예상체결가 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "예상체결가").Trim();
            opt10001VO.예상체결수량 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "예상체결수량").Trim();
            opt10001VO.최고가일250 = DateTime.ParseExact(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "250최고가일").Trim(), "yyyyMMdd", null);
            opt10001VO.최고가대비율250 = float.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "250최고가대비율").Trim());
            opt10001VO.최저가일250 = DateTime.ParseExact(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "250최저가일").Trim(), "yyyyMMdd", null);
            opt10001VO.최저가대비율250 = float.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "250최저가대비율").Trim());
            opt10001VO.현재가 = Math.Abs(int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "현재가").Trim()));
            opt10001VO.대비기호 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "대비기호").Trim();
            opt10001VO.전일대비 = int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "전일대비").Trim());
            opt10001VO.등락율 = float.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "등락율").Trim());
            opt10001VO.거래량 = int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "거래량").Trim());
            opt10001VO.거래대비 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "거래대비").Trim();
            opt10001VO.액면가단위 = axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "액면가단위").Trim();
            opt10001VO.유통주식 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "유통주식").Trim()) ? 0 : int.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "유통주식").Trim());
            opt10001VO.유통비율 = System.String.IsNullOrEmpty(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "유통비율").Trim()) ? 0 : float.Parse(axKHOpenAPI1.GetCommData(sTrCode, sRQName, 0, "유통비율").Trim());

            return opt10001VO;
        }
    }
}
