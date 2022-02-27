using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.SevenBread.model
{
    public class SevenBreadItem
    {
        public long id { get; set; }
        public string itemName { get; set; }
        public string itemCode { get; set; }
        public string theme { get; set; }
        public int closingPrice { get; set; }
        public int capturedPrice { get; set; }
        public double fluctuationRate { get; set; }
        public int priceByYesterday { get; set; }
        public int volume { get; set; }
        public string majorHandler { get; set; }
        public string capturedDate { get; set; }
        public string alarmedTime { get; set; }
        public string alarmStatus { get; set; }
        public int presentPrice { get; set; }
        public double fluctuationRateBy { get; set; }
        public int transactionCount { get; set; } = 0;
    }
}
