using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.RealCondition.model
{
    public class Condition
    {
        public int index { get; set; }
        public string name { get; set; }

        public Condition(int index, string name)
        {
            this.index = index;
            this.name = name;
        }
    }
}
