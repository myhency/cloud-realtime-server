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
    public partial class StockMiner : Form, IRealDataEventHandler, ITrEventHandler, IKiwoomAPI
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private DailyCrawler dailyCrawler;
        private Alrime alrime;
        private TrEventHandler trEventHandler;
        //private RealDataEventHandler realDataEventHandler;
        private OtherFunctions otherFunctions;
        private RealTimeController realTimeController;
        private RealConditionController realConditionController;
        private StockItemController stockItemController;
        private MyTelegramBot myTelegramBot;
        private SevenBreadController sevenBreadController;
        private static DateTime today = DateTime.Now;
        private string strNow = today.ToString("yyyy-MM-dd");

        public StockMiner()
        {
            InitializeComponent();

            axKHOpenAPI1.OnEventConnect += axKHOpenAPI1_OnEventConnect;
            login();
        }

        private void axKHOpenAPI1_OnEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            if (e.nErrCode == 0) //로그인 성공시
            {
                Logger.Info("키움API 로그인성공");

                //텔레그램봇 초기화
                this.myTelegramBot = new MyTelegramBot();
                this.myTelegramBot.sendTextMessageAsyncToSwingBot("키움API 로그인성공");
                //this.myTelegramBot.sendTextMessageAsyncToBot($"🤑 {strNow} 클라우드의 주식 훈련소알리미 출발합니다 🤑");

                initializeSevenBreadService();
                

                ////기타함수 초기화
                //otherFunctions = new OtherFunctions(axKHOpenAPI1);

                ////전체종목리스트 초기화
                //stockItemController = new StockItemController(axKHOpenAPI1);

                ////가격수집 서비스 초기화
                //realTimeController = new RealTimeController(axKHOpenAPI1);

                ////조건검색 서비스 초기화
                //realConditionController = new RealConditionController(axKHOpenAPI1);

                ////realConditionController.sendCondition("유통거래량_코스피", false);
            } 
        }

        private void initializeSevenBreadService()
        {
            //키움API 로그인에 성공했다면 007빵 등록종목이 있는지 확인
            //if 007빵 등록종목이 있다면
            //  종가를 포착일 종가를 업데이트
            //007빵 종목이 등록되어 있는지를 확인하는 kafka consumer는 프로그램시작 시
            //한 번만 확인을 한다.(종목 입력이 장 종료 후 또는 일요일 오후에 됨)
            this.sevenBreadController = new SevenBreadController(axKHOpenAPI1);
            //this.sevenBreadController.getNewSevenBreadItemClosingPriceUpdateThread().Start();
            //여기서 약 1분간 쉬어간 후 다음 프로세스를 진행해야 한다.
            
            //this.sevenBreadController.stopNewSevenBreadItemClosingPriceUpdateThread();
            //this.sevenBreadController.startSevenBreadRealTimeMonitoring();

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

        public void requestOpt10001(string itemCode)
        {
            trEventHandler.requestOpt10001(itemCode);
        }

        public void updateCodeListToGoogleSheet(Opt10001VO opt10001VO)
        {
            dailyCrawler.updateCodeListToGoogleSheet(opt10001VO);
        }

        public void setRealReg(string screenNumber, string itemCode, string fidList, string type)
        {
            //realDataEventHandler.setRealReg(screenNumber, itemCode, fidList, type);
        }

        public void setAlarmList(List<Alarm> alarmList)
        {
            //throw new NotImplementedException();
        }
    }
}
