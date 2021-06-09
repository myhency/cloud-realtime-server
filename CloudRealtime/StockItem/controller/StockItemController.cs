using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.StockItem.controller
{
    public partial class StockItemController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private OtherFunctions otherFunctions;

        public StockItemController(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            otherFunctions = new OtherFunctions(axKHOpenAPI1);

            initialize();
        }

        private void initialize()
        {
            List<string> codeList = otherFunctions.GetCodeList("All");
            Logger.Debug($"종목수는 총 {codeList.Count}개 입니다");
        }
    }
}
