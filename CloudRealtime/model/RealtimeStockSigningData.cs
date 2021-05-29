using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.model
{
    public class RealtimeStockSigningData
    {
        public string 종목코드 { get; set; }
        public string 체결시간 { get; set; }
        public string 현재가 { get; set; }
        public string 전일대비 { get; set; }
        public string 등락율 { get; set; }
        public string 거래량 { get; set; }
        public string 누적거래량 { get; set; }
        public string 누적거래대금 { get; set; }
        public string 시가 { get; set; }
        public string 고가 { get; set; }
        public string 저가 { get; set; }
        public string 전일거래량대비 { get; set; }
        public string 종목명 { get; set; }

        public RealtimeStockSigningData(
            string 종목코드,
            string 체결시간,
            string 현재가,
            string 전일대비,
            string 등락율,
            string 거래량,
            string 누적거래량,
            string 누적거래대금,
            string 시가,
            string 고가,
            string 저가,
            string 전일거래량대비,
            string 종목명)
        {
            this.종목코드 = 종목코드;
            this.체결시간 = 체결시간;
            this.현재가 = 현재가;
            this.전일대비 = 전일대비;
            this.등락율 = 등락율;
            this.거래량 = 거래량;
            this.누적거래량 = 누적거래량;
            this.누적거래대금 = 누적거래대금;
            this.시가 = 시가;
            this.고가 = 고가;
            this.저가 = 저가;
            this.전일거래량대비 = 전일거래량대비;
            this.종목명 = 종목명;
        }
    }
}
