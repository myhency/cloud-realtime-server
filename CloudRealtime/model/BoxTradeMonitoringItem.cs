using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.model
{
    public class BoxTradeMonitoringItem
    {
        public string 기간 { get; set; }
        public string 테마 { get; set; }
        public string 종목 { get; set; }
        public string 상위박스상단 { get; set; }
        public string 현재박스상단 { get; set; }
        public string 현재박스하단 { get; set; }
        public string 하위박스하단 { get; set; }
        public string 일차 { get; set; }
        public string 이차 { get; set; }
        public string 종목코드 { get; set; }
    }
}
