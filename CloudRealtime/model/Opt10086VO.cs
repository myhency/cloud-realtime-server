using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.model
{
    public class Opt10086VO
    {
        public int 종가 { get; set; }
        public int 고가 { get; set; }
        public int 저가 { get; set; }
        public int 외인순매수 { get; set; }
        public int 기관순매수 { get; set; }
        public int 개인순매수 { get; set; }
        public int 전일비 { get; set; }
        public float 등락률 { get; set; }
        public int 거래량 { get; set; }
        public string 날짜 { get; set; }
    }
}
