using CloudRealtime.util;
using NLog;

namespace CloudRealtime.controllers
{
    public partial class CloudController
    {
        protected static Logger logger = LogManager.GetCurrentClassLogger();
        protected MyTelegramBot myTelegramBot;
        protected AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;

        public CloudController(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.myTelegramBot = new MyTelegramBot();
            this.axKHOpenAPI1 = axKHOpenAPI;
        }
    }
}
