using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CloudRealtime.KiwoomAPI;

namespace CloudRealtime
{
    public partial class StockMiner : Form, IDailyCrawler, IAlrime, IRealDataEventHandler, ITrEventHandler, IKiwoomAPI
    {
        private DailyCrawler dailyCrawler;
        private Alrime alrime;
        private TrEventHandler trEventHandler;
        private RealDataEventHandler realDataEventHandler;
        private OtherFunctions otherFunctions;

        public StockMiner()
        {
            InitializeComponent();

            googleSheetRowNumTextBox.Text = "2";

            axKHOpenAPI1.OnEventConnect += axKHOpenAPI1_OnEventConnect;

            login();
        }

        private void axKHOpenAPI1_OnEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            if (e.nErrCode == 0) //로그인 성공시
            {
                Console.WriteLine("로그인성공");
                //데일리종목정보 초기화
                dailyCrawler = new DailyCrawler(this, axKHOpenAPI1);

                //알리미 초기화
                alrime = new Alrime(this, axKHOpenAPI1);

                //TR 요청 이벤트 핸들러 초기화
                trEventHandler = new TrEventHandler(this, axKHOpenAPI1);

                //기타함수 초기화
                otherFunctions = new OtherFunctions(this, axKHOpenAPI1);
            }
        }

        private void login()
        {
            Console.WriteLine("login start");
            axKHOpenAPI1.CommConnect();
        }

        public Button getRequestAllItemInfoButton()
        {
            return requestAllItemInfoButton;
        }

        public Button getRequestKospiItemInfoButton()
        {
            return requestKospiItemInfoButton;
        }

        public Button getRequestKosdaqItemInfoButton()
        {
            return requestKosdaqItemInfoButton;
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

        public TextBox getGoogleSheetRowNumTextBox()
        {
            return googleSheetRowNumTextBox;
        }

        public Button getStartAlarmButton()
        {
            return startAlarmButton;
        }

        public TextBox getLongTermStartColTextBox()
        {
            return longTermStartColTextBox;
        }

        public TextBox getLongTermEndColTextBox()
        {
            return longTermEndColTextBox;
        }

        public TextBox getLongTermStartRowTextBox()
        {
            return longTermStartRowTextBox;
        }

        public TextBox getLongTermEndRowTextBox()
        {
            return longTermEndRowTextBox;
        }

        public TextBox getShortTermStartColTextBox()
        {
            return shortTermStartColTextBox;
        }

        public TextBox getShortTermEndColTextBox()
        {
            return shortTermEndColTextBox;
        }

        public TextBox getShortTermStartRowTextBox()
        {
            return shortTermStartRowTextBox;
        }

        public TextBox getShortTermEndRowTextBox()
        {
            return shortTermEndRowTextBox;
        }

        public void setRealReg(string screenNumber, string itemCode, string fidList, string type)
        {
            realDataEventHandler.setRealReg(screenNumber, itemCode, fidList, type);
        }

        public CheckBox getLongTermActivationCheckBox()
        {
            return longTermActivationCheckBox;
        }

        public CheckBox getSwingActivationCheckBox()
        {
            return swingActivationCheckBox;
        }

        public CheckBox getBoxTradeActivationCheckBox()
        {
            return boxTradeActivationCheckBox;
        }

        public TextBox getBoxTradeStartColTextBox()
        {
            return boxTradeStartColTextBox;
        }

        public TextBox getBoxTradeEndColTextBox()
        {
            return boxTradeEndColTextBox;
        }

        public TextBox getBoxTradeStartRowTextBox()
        {
            return boxTradeStartRowTextBox;
        }

        public TextBox getBoxTradeEndRowTextBox()
        {
            return boxTradeEndRowTextBox;
        }
    }
}
