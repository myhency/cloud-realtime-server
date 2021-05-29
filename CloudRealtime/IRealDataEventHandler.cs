using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime
{
    public interface IRealDataEventHandler
    {
        void setRealReg(string screenNumber, string itemCode, string fidList, string type);
    }
}
