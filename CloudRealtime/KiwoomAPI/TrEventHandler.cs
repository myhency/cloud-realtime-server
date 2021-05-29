using CloudRealtime.model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudRealtime
{
    public partial class TrEventHandler
    {
        private static string mysqlConnStr = "Server=localhost;Database=stockdb;Uid=root;Pwd=037603;Charset=utf8";
        private MySqlConnection conn = new MySqlConnection(mysqlConnStr);
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private IDailyCrawler iDailyCrawler;
        private ITrader iTrader;
        private int screenNumber = 1000;
        private int trRequestCount = 0;

        public TrEventHandler(IDailyCrawler iDailyCrawler, AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.iDailyCrawler = iDailyCrawler;
            axKHOpenAPI1.OnReceiveTrData += axKHOpenAPI1_OnReceiveTrData;

            try
            {
                conn.Open();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message.ToString());
            }
        }

        public TrEventHandler(ITrader iTrader, AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.iTrader = iTrader;
            axKHOpenAPI1.OnReceiveTrData += axKHOpenAPI1_OnReceiveTrData;
        }

        public void requestOpt10001(string itemCode)
        {
            axKHOpenAPI1.SetInputValue("종목코드",itemCode);
            axKHOpenAPI1.CommRqData("주식기본정보요청", "opt10001", 0, screenNumber.ToString());
        }

        private void axKHOpenAPI1_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            if (e.sRQName.Equals("주식기본정보요청"))
            {
                Opt10001VO opt10001VO = getOpt10001VO(e.sTrCode, e.sRQName);

                insertKiwoomStockCrawlerDaily(opt10001VO);

                iDailyCrawler.updateCodeListToGoogleSheet(opt10001VO);
            }
        }

        private void insertKiwoomStockCrawlerDaily(Opt10001VO opt10001VO)
        {
            string query = "INSERT INTO stockdb.kiwoom_stock_crawler_daily" +
                                "(" +
                                "item_code," +
                                "item_name," +
                                "settlement_month," +
                                "face_value," +
                                "capital," +
                                "num_of_listed_shares," +
                                "credit_ratio," +
                                "highest_year_round," +
                                "lowest_year_round," +
                                "market_capitalization," +
                                "external_burnout_rate," +
                                "substitute_stock," +
                                "per," +
                                "eps," +
                                "roe," +
                                "pbr," +
                                "bps," +
                                "take," +
                                "operating_profit," +
                                "net_income," +
                                "highest_250," +
                                "lowest_250," +
                                "staring_price," +
                                "highest_price," +
                                "lowest_price," +
                                "upper_limit," +
                                "lower_limit," +
                                "reference_price," +
                                "highest_250_at," +
                                "highest_250_ratio," +
                                "lowest_250_at," +
                                "lowest_250_ratio," +
                                "present_price," +
                                "price_state_flag," +
                                "price_by_prev_day," +
                                "fluctuation_rate," +
                                "volume," +
                                "volume_by_prev_day," +
                                "stocks_in_circulation," +
                                "circulation_ratio," +
                                "created_at)" +
                                " VALUES (" +
                                $"'{opt10001VO.종목코드}'," +
                                $"'{opt10001VO.종목명}'," +
                                $"'{opt10001VO.결산월}'," +
                                $"{opt10001VO.액면가}," +
                                $"{opt10001VO.자본금}," +
                                $"{opt10001VO.상장주식}," +
                                $"{opt10001VO.신용비율}," +
                                $"{opt10001VO.연중최고}," +
                                $"{opt10001VO.연중최저}," +
                                $"{opt10001VO.시가총액}," +
                                $"{opt10001VO.외인소진률}," +
                                $"{opt10001VO.대용가}," +
                                $"{opt10001VO.PER}," +
                                $"{opt10001VO.EPS}," +
                                $"{opt10001VO.ROE}," +
                                $"{opt10001VO.PBR}," +
                                $"{opt10001VO.BPS}," +
                                $"{opt10001VO.매출액}," +
                                $"{opt10001VO.영업이익}," +
                                $"{opt10001VO.당기순이익}," +
                                $"{opt10001VO.최고250}," +
                                $"{opt10001VO.최저250}," +
                                $"{opt10001VO.시가}," +
                                $"{opt10001VO.고가}," +
                                $"{opt10001VO.저가}," +
                                $"{opt10001VO.상한가}," +
                                $"{opt10001VO.하한가}," +
                                $"{opt10001VO.기준가}," +
                                $"STR_TO_DATE('{opt10001VO.최고가일250.ToString("yyyyMMdd")}','%Y%m%d')," +
                                $"{opt10001VO.최고가대비율250}," +
                                $"STR_TO_DATE('{opt10001VO.최저가일250.ToString("yyyyMMdd")}','%Y%m%d')," +
                                $"{opt10001VO.최저가대비율250}," +
                                $"{opt10001VO.현재가}," +
                                $"{opt10001VO.대비기호}," +
                                $"{opt10001VO.전일대비}," +
                                $"{opt10001VO.등락율}," +
                                $"{opt10001VO.거래량}," +
                                $"{opt10001VO.거래대비}," +
                                $"{opt10001VO.유통주식}," +
                                $"{opt10001VO.유통비율}," +
                                $"STR_TO_DATE('{DateTime.Now.ToString("yyyyMMdd")}','%Y%m%d')" +
                                ")";
            Console.WriteLine(query);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message.ToString());
            }
            finally
            {
                cmd.Dispose();
            }
        }

        private Opt10001VO getOpt10001VO(string sTrCode, string sRQName)
        {
            Opt10001VO opt10001VO = new Opt10001VO();
            try
            {
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

            } catch(Exception exception)
            {
                Console.WriteLine(exception.Message.ToString());
            }


            return opt10001VO;
        }
    }
}
