using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.SevenBread.model
{
    public class SevenBreadItemCreationRequestDto
    {
        public string itemName { get; set; }
        public string itemCode { get; set; }
        public string majorHandler { get; set; }
        public string capturedDate { get; set; }
        public string theme { get; set; }
    }
}
