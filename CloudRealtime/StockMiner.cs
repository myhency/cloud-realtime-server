using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CloudRealtime.KiwoomAPI;
using CloudRealtime.RealCondition.controller;
using CloudRealtime.RealTime.controller;
using CloudRealtime.RealTime.model;
using CloudRealtime.SevenBread.controller;
using CloudRealtime.StockItem.controller;
using CloudRealtime.util;
using NLog;

namespace CloudRealtime
{
    public partial class StockMiner : Form, ITrEventHandler, IKiwoomAPI
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private DailyCrawler dailyCrawler;
        //private Alrime alrime;
        //private TrEventHandler trEventHandler;
        //private RealDataEventHandler realDataEventHandler;
        private OtherFunctions otherFunctions;
        private RealTimeController realTimeController;
        private RealConditionController realConditionController;
        private StockItemController stockItemController;
        private MyTelegramBot myTelegramBot;
        private SevenBreadController sevenBreadController;
        private static DateTime today = DateTime.Now;

        public StockMiner()
        {
            InitializeComponent();

            this.myTelegramBot = new MyTelegramBot();

            axKHOpenAPI1.OnEventConnect += axKHOpenAPI1_OnEventConnect;
            login();
        }

        private void axKHOpenAPI1_OnEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            DateTime today = DateTime.Now;
            DateTime startMarketTime = new DateTime(today.Year, today.Month, today.Day, 09, 0, 0);
            string strNow = today.ToString("yyyy년 MM월 dd일");

            if (e.nErrCode == 0) //로그인 성공시
            {
                Logger.Info("키움API 로그인성공");

                //시작메세지 발송
                this.myTelegramBot.sendTextMessageAsyncToSwingBot("키움API 로그인성공");
                //this.myTelegramBot.sendTextMessageAsyncToBot($"🤑 {strNow} 클라우드의 주식 훈련소알리미 출발합니다 🤑");

                //전체종목리스트 초기화
                stockItemController = new StockItemController(axKHOpenAPI1);

                //007빵 초기화
                this.sevenBreadController = new SevenBreadController(axKHOpenAPI1);

                //가격수집 서비스 초기화
                this.realTimeController = new RealTimeController(axKHOpenAPI1);

                //조건검색 서비스 초기화
                this.realConditionController = new RealConditionController(axKHOpenAPI1);

                ////realConditionController.sendCondition("유통거래량_코스피", false);
            }
        }

        private void login()
        {
            //Logger.Debug("login start");
            axKHOpenAPI1.CommConnect();
        }

        public List<string> getCodeList(string market)
        {
           return otherFunctions.GetCodeList(market);
        }

        public void updateCodeListToGoogleSheet(Opt10001VO opt10001VO)
        {
            dailyCrawler.updateCodeListToGoogleSheet(opt10001VO);
        }
    }
}
