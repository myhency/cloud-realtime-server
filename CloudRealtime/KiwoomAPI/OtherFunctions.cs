using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudRealtime.KiwoomAPI;

namespace CloudRealtime
{
    public partial class OtherFunctions
    {
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private IKiwoomAPI kiwoomAPI;

        public OtherFunctions(IKiwoomAPI kiwoomAPI, AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.kiwoomAPI = kiwoomAPI;
            this.axKHOpenAPI1 = axKHOpenAPI;
        }

        public List<string> GetCodeList(string market)
        {
            List<string> result = new List<string>();
            string[] kospiItemCodeArray;
            string[] kosdaqItemCodeArray;

            switch (market) {
                case "All":
                    kospiItemCodeArray = this.axKHOpenAPI1.GetCodeListByMarket("0").TrimEnd(';').Split(';');
                    kosdaqItemCodeArray = this.axKHOpenAPI1.GetCodeListByMarket("10").TrimEnd(';').Split(';');
                    foreach (string itemCode in kospiItemCodeArray.Concat(kosdaqItemCodeArray).ToArray())
                    {
                        result.Add(itemCode);
                    }
                    break;
                case "Kospi":
                    kospiItemCodeArray = this.axKHOpenAPI1.GetCodeListByMarket("0").TrimEnd(';').Split(';');
                    foreach (string itemCode in kospiItemCodeArray)
                    {
                        result.Add(itemCode);
                    }
                    break;
                case "Kosdaq":
                    kosdaqItemCodeArray = this.axKHOpenAPI1.GetCodeListByMarket("10").TrimEnd(';').Split(';');
                    foreach (string itemCode in kosdaqItemCodeArray)
                    {
                        result.Add(itemCode);
                    }
                    break;
                default:
                    break;
            }
            
            return result;
        }
    }
}
