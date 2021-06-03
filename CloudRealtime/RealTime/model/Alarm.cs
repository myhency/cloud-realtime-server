using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.RealTime.model
{
    public class Alarm
    {
        public long alarmId { get; set; }
        public string itemName { get; set; }
        public string itemCode { get; set; }
        public int recommendPrice { get; set; }
        public int losscutPrice { get; set; }
        public string theme { get; set; }
        public string comment { get; set; }
        public string alarmStatus { get; set; }
    }
}
