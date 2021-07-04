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
        private static DateTime today = DateTime.Now;
        private string strNow = today.ToString("yyyy-MM-dd");

        public StockMiner()
        {
            InitializeComponent();

            axKHOpenAPI1.OnEventConnect += axKHOpenAPI1_OnEventConnect;

            //Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory + "logs" + Path.DirectorySeparatorChar);
            //var watcher = new FileSystemWatcher();
            //watcher.Path = AppDomain.CurrentDomain.BaseDirectory + "logs";
            //watcher.NotifyFilter = NotifyFilters.LastWrite;
            //watcher.Filter = $"{strNow}.log";
            //watcher.Changed += new FileSystemEventHandler(Changed);
            //watcher.EnableRaisingEvents = true;
            login();
        }

        //private void Changed(object sender, FileSystemEventArgs e)
        //{   
        //    var log = File.ReadLines(
        //        AppDomain.CurrentDomain.BaseDirectory
        //        + "logs"
        //        + Path.DirectorySeparatorChar
        //        + $"{strNow}.log",
        //        System.Text.Encoding.GetEncoding(949)
        //        )
        //        .Last();
        //    this.Invoke(new Action(delegate ()
        //    {
        //        logRichTextBox.AppendText(log + "\n");
        //    }));
        //}

        private void axKHOpenAPI1_OnEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            if (e.nErrCode == 0) //로그인 성공시
            {
                Logger.Debug("로그인성공");
                //데일리종목정보 초기화
                //dailyCrawler = new DailyCrawler(this, axKHOpenAPI1);

                //알리미 초기화
                //alrime = new Alrime(this, axKHOpenAPI1);

                //TR 요청 이벤트 핸들러 초기화
                //trEventHandler = new TrEventHandler(this, axKHOpenAPI1);

                //기타함수 초기화
                otherFunctions = new OtherFunctions(axKHOpenAPI1);

                //가격수집 서비스 초기화
                realTimeController = new RealTimeController(axKHOpenAPI1);

                //조건검색 서비스 초기화
                realConditionController = new RealConditionController(axKHOpenAPI1);

                //realConditionController.sendCondition("유통거래량_코스피", false);
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
