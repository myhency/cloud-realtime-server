using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.SevenBread.model
{
    public class SevenBreadDeletedItem
    {
        public long id { get; set; }
        public string itemName { get; set; }
        public string itemCode { get; set; }
        public string majorHandler { get; set; }
        public string capturedDate { get; set; }
        public int capturedPrice { get; set; }
    }
}
