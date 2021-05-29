using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Dynamic;

namespace CloudRealtime
{
    
    public partial class DailyCrawler
    {
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private IDailyCrawler iDailyCrawler;
        private int rangeRowStart = 2;
        private string sheetName;

        private Button requestAllItemInfoButton;
        private Button requestKospiItemInfoButton;
        private Button requestKosdaqItemInfoButton;
        private TextBox googleSheetRowNumTextBox;

        public DailyCrawler(IDailyCrawler iDailyCrawler, AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.iDailyCrawler = iDailyCrawler;
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.requestAllItemInfoButton = iDailyCrawler.getRequestAllItemInfoButton();
            this.requestKospiItemInfoButton = iDailyCrawler.getRequestKospiItemInfoButton();
            this.requestKosdaqItemInfoButton = iDailyCrawler.getRequestKosdaqItemInfoButton();
            this.googleSheetRowNumTextBox = iDailyCrawler.getGoogleSheetRowNumTextBox();
            this.requestAllItemInfoButton.Click += requestAllItemInfoButton_Click;
            this.requestKospiItemInfoButton.Click += requestKospiItemInfoButton_Click;
            this.requestKosdaqItemInfoButton.Click += requestKosdaqItemInfoButton_Click;
            this.googleSheetRowNumTextBox.Text = rangeRowStart.ToString();
        }

        private void requestAllItemInfoButton_Click(object sender, EventArgs e)
        {
            rangeRowStart = int.Parse(this.googleSheetRowNumTextBox.Text.Trim());
            sheetName = "Kospi-Kosdaq";

            List<string> codeList = iDailyCrawler.getCodeList("All");

            var toBeRemoved = getToBeRemovedItemsFromGoogleSheet();
            foreach(var item in toBeRemoved)
            {
                var dict = (IDictionary<string, object>)item;
                foreach(var d in dict)
                {
                    codeList.Remove(d.Value.ToString());
                }
            }

            Console.WriteLine($"종목수는 총 {codeList.Count}개 입니다.");

            for (int i = rangeRowStart - 2; i < codeList.Count; i++)
            {
                iDailyCrawler.requestOpt10001(codeList[i]);
            }
        }

        private void requestKospiItemInfoButton_Click(object sender, EventArgs e)
        {
            rangeRowStart = int.Parse(this.googleSheetRowNumTextBox.Text.Trim());
            sheetName = "Kospi";

            List<string> codeList = iDailyCrawler.getCodeList("Kospi");
            Console.WriteLine($"종목수는 총 {codeList.Count}개 입니다.");
            for (int i = rangeRowStart - 2; i < codeList.Count; i++)
            {
                iDailyCrawler.requestOpt10001(codeList[i]);
            }
        }

        private void requestKosdaqItemInfoButton_Click(object sender, EventArgs e)
        {
            rangeRowStart = int.Parse(this.googleSheetRowNumTextBox.Text.Trim());
            sheetName = "Kosdaq";

            List<string> codeList = iDailyCrawler.getCodeList("Kosdaq");
            Console.WriteLine($"종목수는 총 {codeList.Count}개 입니다.");
            for (int i = rangeRowStart - 2; i < codeList.Count; i++)
            {
                iDailyCrawler.requestOpt10001(codeList[i]);
            }
        }

        private List<ExpandoObject> getToBeRemovedItemsFromGoogleSheet()
        {
            var gsh = new GoogleSheetsHelper("swing-293507-ca9c2651b2d1.json", "17rL1vpCWgN62XhlSnlObgv0pVwc9KRbJdHDYCSRQtMc");
            var gsp = new GoogleSheetParameters() { RangeColumnStart = 3, RangeRowStart = 1, RangeColumnEnd = 3, RangeRowEnd=1106, FirstRowIsHeaders = true, SheetName = "ToBeRemoved" };
            var rowValues = gsh.GetDataFromSheet(gsp);
            return rowValues;
        }

        public void updateCodeListToGoogleSheet(Opt10001VO opt10001VO)
        {
            var gsh = new GoogleSheetsHelper("swing-293507-ca9c2651b2d1.json", "17rL1vpCWgN62XhlSnlObgv0pVwc9KRbJdHDYCSRQtMc");
            var row = new GoogleSheetRow();
            var cells = new List<GoogleSheetCell>();
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.종목명 });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.종목코드 });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.결산월 });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.액면가.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.자본금.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.상장주식.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.신용비율.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.연중최고.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.연중최저.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.시가총액.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.시가총액비중 });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.외인소진률.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.대용가.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.PER.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.EPS.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.ROE.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.PBR.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.EV.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.BPS.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.매출액.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.영업이익.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.당기순이익.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.최고250.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.최저250.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.시가.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.고가.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.저가.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.상한가.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.하한가.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.기준가.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.예상체결가 });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.예상체결수량 });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.최고가일250.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.최고가대비율250.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.최저가일250.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.최저가대비율250.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.현재가.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.대비기호 });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.전일대비.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.등락율.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.거래량.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.거래대비.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.액면가단위 });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.유통주식.ToString() });
            cells.Add(new GoogleSheetCell() { CellValue = opt10001VO.유통비율.ToString() });
            row.Cells.AddRange(cells);
            var rows = new List<GoogleSheetRow>() { row };
            gsh.AddCells(new GoogleSheetParameters() { SheetName = sheetName, RangeColumnStart = 1, RangeRowStart = rangeRowStart }, rows);
            rangeRowStart++;
            Console.WriteLine($"{opt10001VO.종목명} 업데이트 완료 ({rangeRowStart})");
        }
    }
}
