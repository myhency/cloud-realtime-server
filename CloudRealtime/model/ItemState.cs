using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.model
{
    public class ItemState
    {
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public string curStep { get; set; } = null;
        public int avgUnitPrice { get; set; }
        public int quantity { get; set; }
        public double weight { get; set; } = 0;
        public double yield { get; set; }
        public bool isStep0Trade { get; set; } = false;
        public bool isStep1Trade { get; set; } = false;
        public bool isStep2Trade { get; set; } = false;
        public bool isStep3Trade { get; set; } = false;
    }
}
