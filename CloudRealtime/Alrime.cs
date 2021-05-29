using CloudRealtime.util;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CloudRealtime.model;

namespace CloudRealtime
{
    public partial class Alrime : ITrader
    {
        private IAlrime iAlrime;
        private MyTelegramBot myTelegramBot;
        private RealDataEventHandler realDataEventHandler;
        private TrEventHandler trEventHandler;
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private List<MonitoringItem> longTermMonitoringItemList = new List<MonitoringItem>();
        private List<MonitoringItem> swingMonitoringItemList = new List<MonitoringItem>();
        private List<BoxTradeMonitoringItem> boxTradeMonitoringItemList = new List<BoxTradeMonitoringItem>();
        private string toBeRemoved = null;
        private List<ItemState> itemStateList = new List<ItemState>();

        IniFile ini = new IniFile();

        private Button startAlarmButton;
        private TextBox longTermStartColTextBox;
        private TextBox longTermEndColTextBox;
        private TextBox longTermStartRowTextBox;
        private TextBox longTermEndRowTextBox;
        private TextBox shortTermStartColTextBox;
        private TextBox shortTermEndColTextBox;
        private TextBox shortTermStartRowTextBox;
        private TextBox shortTermEndRowTextBox;
        private TextBox boxTradeStartColTextBox;
        private TextBox boxTradeEndColTextBox;
        private TextBox boxTradeStartRowTextBox;
        private TextBox boxTradeEndRowTextBox;
        private CheckBox longTermActivationCheckBox;
        private CheckBox swingActivationCheckBox;
        private CheckBox boxTradeActivationCheckBox;
        public Alrime(IAlrime iAlrime, AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.iAlrime = iAlrime;
            this.axKHOpenAPI1 = axKHOpenAPI;
            startAlarmButton = iAlrime.getStartAlarmButton();
            longTermStartColTextBox = iAlrime.getLongTermStartColTextBox();
            longTermEndColTextBox = iAlrime.getLongTermEndColTextBox();
            longTermStartRowTextBox = iAlrime.getLongTermStartRowTextBox();
            longTermEndRowTextBox = iAlrime.getLongTermEndRowTextBox();
            shortTermStartColTextBox = iAlrime.getShortTermStartColTextBox();
            shortTermEndColTextBox = iAlrime.getShortTermEndColTextBox();
            shortTermStartRowTextBox = iAlrime.getShortTermStartRowTextBox();
            shortTermEndRowTextBox = iAlrime.getShortTermEndRowTextBox();
            boxTradeStartColTextBox = iAlrime.getBoxTradeStartColTextBox();
            boxTradeEndColTextBox = iAlrime.getBoxTradeEndColTextBox();
            boxTradeStartRowTextBox = iAlrime.getBoxTradeStartRowTextBox();
            boxTradeEndRowTextBox = iAlrime.getBoxTradeEndRowTextBox();
            longTermActivationCheckBox = iAlrime.getLongTermActivationCheckBox();
            swingActivationCheckBox = iAlrime.getSwingActivationCheckBox();
            boxTradeActivationCheckBox = iAlrime.getBoxTradeActivationCheckBox();
            startAlarmButton.Click += startAlarmButton_Click;

            ini.Load(Application.StartupPath + "\\settings.ini");
            longTermStartColTextBox.Text = ini["LongTerm"]["RangeColumnStart"].ToString();
            longTermEndColTextBox.Text = ini["LongTerm"]["RangeColumnEnd"].ToString();
            longTermStartRowTextBox.Text = ini["LongTerm"]["RangeRowStart"].ToString();
            longTermEndRowTextBox.Text = ini["LongTerm"]["RangeRowEnd"].ToString();
            shortTermStartColTextBox.Text = ini["ShortTerm"]["RangeColumnStart"].ToString();
            shortTermEndColTextBox.Text = ini["ShortTerm"]["RangeColumnEnd"].ToString();
            shortTermStartRowTextBox.Text = ini["ShortTerm"]["RangeRowStart"].ToString();
            shortTermEndRowTextBox.Text = ini["ShortTerm"]["RangeRowEnd"].ToString();
            boxTradeStartColTextBox.Text = ini["BoxTrade"]["RangeColumnStart"].ToString();
            boxTradeEndColTextBox.Text = ini["BoxTrade"]["RangeColumnEnd"].ToString();
            boxTradeStartRowTextBox.Text = ini["BoxTrade"]["RangeRowStart"].ToString();
            boxTradeEndRowTextBox.Text = ini["BoxTrade"]["RangeRowEnd"].ToString();

            realDataEventHandler = new RealDataEventHandler(this,axKHOpenAPI);
            //trEventHandler = new TrEventHandler(this, axKHOpenAPI);
        }

        private void startAlarmButton_Click(object sender, EventArgs e)
        {
            //텔레그램 봇 초기화
            myTelegramBot = new MyTelegramBot();

            //구글스프레드시트 데이터 가져오기 & 실시간 감시 등록하기
            if (longTermActivationCheckBox.Checked)
            {
                longTermMonitoringItemList = getLongTermSheet();
            }

            if (boxTradeActivationCheckBox.Checked)
            {
                boxTradeMonitoringItemList = getBoxTradeSheet();
                for(int i = 0; i < boxTradeMonitoringItemList.Count; i++)
                {
                    /// FIDs ///
                    /// 20:체결시간
                    /// 10:현재가 
                    /// 11:전일대비
                    /// 12:등락율
                    /// 15:거래량
                    /// 13:누적거래량
                    /// 14:누적거래대금
                    /// 16:시가
                    /// 17:고가
                    /// 18:저가
                    /// 30:전일거래량대비(계약,주)
                    realDataEventHandler.setRealReg(
                        "5000", 
                        boxTradeMonitoringItemList[i].종목코드, 
                        "20;10;11;12;15;13;14;16;17;18;30,290,9201", 
                        "1"
                    );
                }
            }
        }

        private List<BoxTradeMonitoringItem> getBoxTradeSheet()
        {
            var gsh = new GoogleSheetsHelper("swing-293507-ca9c2651b2d1.json", "17rL1vpCWgN62XhlSnlObgv0pVwc9KRbJdHDYCSRQtMc");
            var values = gsh.GetDataFromSheet(new GoogleSheetParameters()
            {
                SheetName = "BoxTrade",
                RangeColumnStart = int.Parse(boxTradeStartColTextBox.Text.ToString()),
                RangeColumnEnd = int.Parse(boxTradeEndColTextBox.Text.ToString()),
                RangeRowStart = int.Parse(boxTradeStartRowTextBox.Text.ToString()),
                RangeRowEnd = int.Parse(boxTradeEndRowTextBox.Text.ToString()),
                FirstRowIsHeaders = true
            });

            //var dict = (IDictionary<string, object>)values[0];
            List<BoxTradeMonitoringItem> monitoringItemList = new List<BoxTradeMonitoringItem>();

            foreach (var value in values)
            {
                var dict = (IDictionary<string, object>)value;
                BoxTradeMonitoringItem monitoringItem = new BoxTradeMonitoringItem();
                foreach (var d in dict)
                {
                    //Console.WriteLine($"{d.Key} {d.Value}");
                    monitoringItem.기간 = "BoxTrade";
                    if (d.Key.Equals("테마")) monitoringItem.테마 = d.Value.ToString();
                    else if (d.Key.Equals("종목")) monitoringItem.종목 = d.Value.ToString();
                    else if (d.Key.Equals("상위박스 상단")) monitoringItem.상위박스상단 = d.Value.ToString();
                    else if (d.Key.Equals("현재박스 상단")) monitoringItem.현재박스상단 = d.Value.ToString();
                    else if (d.Key.Equals("현재박스 하단")) monitoringItem.현재박스하단 = d.Value.ToString();
                    else if (d.Key.Equals("하위박스 하단")) monitoringItem.하위박스하단 = d.Value.ToString();
                    else if (d.Key.Equals("1차")) monitoringItem.일차 = d.Value.ToString();
                    else if (d.Key.Equals("2차")) monitoringItem.이차 = d.Value.ToString();
                    else if (d.Key.Equals("종목코드")) monitoringItem.종목코드 = d.Value.ToString();
                    // TODO: 테마별 비중 계산하기 -> 전체테마개수:테마별종목매수합계 = 100:15
                    // TODO: 각 테마별 종목 비중 계산하기 -> 테마매수합산:개별종목매수금 = 100:개별종목매수금/태마내 종목수

                    //iRealDataEventHandler.setRealReg("3000", d.Value.ToString(), "20;10;11;12;15;13;14;16;17;18;30", "1");
                }
                monitoringItemList.Add(monitoringItem);
            }

            return monitoringItemList;
        }

        private List<MonitoringItem> getLongTermSheet()
        {
            var gsh = new GoogleSheetsHelper("swing-293507-ca9c2651b2d1.json", "17rL1vpCWgN62XhlSnlObgv0pVwc9KRbJdHDYCSRQtMc");
            var values = gsh.GetDataFromSheet(new GoogleSheetParameters()
            {
                SheetName = "LongTerm",
                RangeColumnStart = int.Parse(longTermStartColTextBox.Text.ToString()),
                RangeColumnEnd = int.Parse(longTermEndColTextBox.Text.ToString()),
                RangeRowStart = int.Parse(longTermStartRowTextBox.Text.ToString()),
                RangeRowEnd = int.Parse(longTermEndRowTextBox.Text.ToString()),
                FirstRowIsHeaders = true
            });

            //var dict = (IDictionary<string, object>)values[0];
            List<MonitoringItem> monitoringItemList = new List<MonitoringItem>();

            foreach(var value in values)
            {
                var dict = (IDictionary<string, object>)value;
                MonitoringItem monitoringItem = new MonitoringItem();
                foreach (var d in dict)
                {
                    //Console.WriteLine($"{d.Key} {d.Value}");
                    monitoringItem.기간 = "LongTerm";
                    if(d.Key.Equals("테마")) monitoringItem.테마 = d.Value.ToString();
                    else if(d.Key.Equals("종목")) monitoringItem.종목 = d.Value.ToString();
                    else if (d.Key.Equals("1차")) monitoringItem.일차 = d.Value.ToString();
                    else if (d.Key.Equals("2차")) monitoringItem.이차 = d.Value.ToString();
                    else if (d.Key.Equals("손절")) monitoringItem.손절 = d.Value.ToString();
                    else if (d.Key.Equals("종목코드")) monitoringItem.종목코드 = d.Value.ToString();
                    // TODO: 테마별 비중 계산하기 -> 전체테마개수:테마별종목매수합계 = 100:15
                    // TODO: 각 테마별 종목 비중 계산하기 -> 테마매수합산:개별종목매수금 = 100:개별종목매수금/태마내 종목수
                        
                    //iRealDataEventHandler.setRealReg("3000", d.Value.ToString(), "20;10;11;12;15;13;14;16;17;18;30", "1");
                }
                monitoringItemList.Add(monitoringItem);
            }

            return monitoringItemList;
        }

        public void setRealtimeStockSigningData(RealtimeStockSigningData realtimeStockSigningData)
        {
            if(toBeRemoved != null)
            {
                boxTradeMonitoringItemList.RemoveAt(int.Parse(toBeRemoved));
                toBeRemoved = null;
            }
            //Console.WriteLine(realtimeStockSigningData.현재가);
            for(int i = 0; i < boxTradeMonitoringItemList.Count; i++)
            {
                if (boxTradeMonitoringItemList[i].종목코드 == realtimeStockSigningData.종목코드)
                {
                    int presentPrice = Math.Abs(int.Parse(realtimeStockSigningData.현재가.Replace(",", "")));
                    int 일차매수가 = Math.Abs(int.Parse(boxTradeMonitoringItemList[i].일차.Replace(",", "")));
                    int 이차매수가 = Math.Abs(int.Parse(boxTradeMonitoringItemList[i].이차.Replace(",", "")));
                    int 현재박스상단 = Math.Abs(int.Parse(boxTradeMonitoringItemList[i].현재박스상단.Replace(",", "")));
                    int 현재박스하단 = Math.Abs(int.Parse(boxTradeMonitoringItemList[i].현재박스하단.Replace(",", "")));
                    int 하위박스하단 = Math.Abs(int.Parse(boxTradeMonitoringItemList[i].하위박스하단.Replace(",", "")));

                    if (presentPrice >= 현재박스상단 && presentPrice <= 현재박스상단 * 1.01)
                    {
                        step0Trade(realtimeStockSigningData.종목코드, presentPrice);
                    }
                    else if (presentPrice <= 일차매수가 && presentPrice > 현재박스하단)
                    {
                        //step1Trade(realtimeStockSigningData.종목코드, realtimeStockSigningData.현재가);
                    }
                    else if (presentPrice <= 이차매수가 && presentPrice >= 하위박스하단)
                    {
                        //step2Trade(realtimeStockSigningData.종목코드, realtimeStockSigningData.현재가);
                    }
                    else if (presentPrice < 하위박스하단)
                    {
                        //step3Trade(realtimeStockSigningData.종목코드, realtimeStockSigningData.현재가);
                    }
                }
            }
        }

        private void step0Trade(string itemCode, int newPrice)
        {
            ItemState itemState = itemStateList.Find(item => item.itemCode.Equals(itemCode));
            if (itemState.weight >= 10) //비중이 10% 이상이면 안삼 
            {
                return;
            }

            double eachQuantityCanBuy = ((2000000 * (10 -itemState.weight)) / newPrice) / 5;
            int quoteUnit = getQuoteUnit(itemCode, newPrice);

            for (int i = 1; i < 9; i++)
            { 
                if (i == 1)
                {
                    //pbuy(itemCode, newPrice, eachQuantityCanBuy);
                }
                else if (i % 2 == 0) 
                {
                    //buy(itemCode, newPrice - quoteUnit * i, eachQuantityCanBuy);
                }
            }
        }

        private int getQuoteUnit(string itemCode, int newPrice)
        {
            int quoteUnit = 0;
            string[] kospiItemCodeArray = this.axKHOpenAPI1.GetCodeListByMarket("0").TrimEnd(';').Split(';');
            string marketType = "";
            if (kospiItemCodeArray.Contains(itemCode)) marketType = "Kospi";
            else marketType = "Kosdaq";

            if (newPrice < 1000)
            {
                quoteUnit = 1;
            }
            else if (newPrice >= 1000 && newPrice < 5000)
            {
                quoteUnit = 5;
            }
            else if (newPrice >= 5000 && newPrice < 10000)
            {
                quoteUnit = 10;
            }
            else if (newPrice >= 10000 && newPrice < 50000)
            {
                quoteUnit = 50;
            }
            else if (newPrice >= 50000 && newPrice < 100000)
            {
                quoteUnit = 100;
            }
            else if (newPrice >= 100000 && newPrice < 500000)
            {
                if (marketType.Equals("Kospi"))
                {
                    quoteUnit = 500;
                }
                else
                {
                    quoteUnit = 100;
                }
            }
            else if (newPrice > 500000)
            {
                if (marketType.Equals("Kospi"))
                {
                    quoteUnit = 1000;
                }
                else
                {
                    quoteUnit = 100;
                }
            }

            return quoteUnit;
        }
    }
}

