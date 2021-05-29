using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.model
{
    public class MonitoringItem
    {
        public string 기간 { get; set; }
        public string 테마 { get; set; }
        public string 종목 { get; set; }
        public string 일차 { get; set; }
        public string 이차 { get; set; }
        public string 손절 { get; set; }
        public string 보유여부 { get; set; }
        public string 평단가 { get; set; }
        public string 보유수량 { get; set; }
        public string 매수금 { get; set; }
        public string 비중 { get; set; }
        public string 현재가 { get; set; }
        public string 현재가대비손익 { get; set; }
        public string 일차대비현재가 { get; set; }
        public string 이차대비현재가 { get; set; }
        public string 손절가대비현재가 { get; set; }
        public string 종목코드 { get; set; }
    }
}
