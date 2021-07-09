using CloudRealtime.StockItem.model;
using CloudRealtime.StockItem.service;
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
        private StockItemService stockItemService;

        public StockItemController(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            otherFunctions = new OtherFunctions(axKHOpenAPI1);
            this.stockItemService = new StockItemService();

            initialize();
        }

        private void initialize()
        {
            List<Item> marketItemList = new List<Item>();
            List<Item> unMatchedItemList = new List<Item>();
            List<string> codeList = otherFunctions.GetCodeList("All");
            List<Item> itemList = stockItemService.getRegisterItems();

            Logger.Debug($"종목수는 총 {codeList.Count}개 입니다");

            foreach (string item in codeList)
            {
                marketItemList.Add(new Item { itemCode = item, itemName = otherFunctions.GetMasterCodeName(item) });
            }
            
            foreach(Item item in marketItemList)
            {
                Item matchedItem = itemList.FirstOrDefault(v => v.itemCode == item.itemCode);

                if (matchedItem is null)
                {
                    if(item.itemName.Contains(" ETN")
                        || item.itemName.Contains("TIGER ")
                        || item.itemName.Contains("KODEX ")
                        || item.itemName.Contains("KBSTAR ")
                        || item.itemName.Contains("ARIRANG ")
                        || item.itemName.Contains("KINDEX ")
                        || item.itemName.Contains("HANARO ")
                        || item.itemName.Contains("KOSEF ")
                        || item.itemName.Contains("SMART ")
                        || item.itemName.Contains("레버리지")
                        || item.itemName.Contains("네비게이터 ")
                        || item.itemName.Contains("HK ")
                        || item.itemName.Contains("TIMEFOLIO ")
                        || item.itemName.Contains("FOCUS ")
                        || item.itemName.Contains("메리츠 ")
                        || item.itemName.Contains("TREX ")
                        || item.itemName.Contains("파워 ")
                        || item.itemName.Contains("마이다스 ")
                        || item.itemName.Contains("한국ANKOR")
                        || item.itemName.Contains("하이골드")
                        || item.itemName.Contains("바다로")
                        || item.itemName.Contains("마이티 ")
                        || item.itemName.Contains("KTOP ")
                        || item.itemName.Contains("흥국 ")
                        || item.itemName.Contains("CNT85")
                        || item.itemName.Contains("케이프이에스제4호")
                        || item.itemName.Contains("1우")
                        || item.itemName.Contains("2우")
                        || item.itemName.Contains("3우")
                        || item.itemName.Contains("우B")
                        || item.itemName.Contains("호스팩")
                        || (item.itemName.Contains("스팩") && item.itemName.EndsWith("호"))
                    ) //ETN, ETF, 스팩 제외
                    {
                        goto End;
                    }

                    Logger.Info($"등록되지 않은 종목: 종목명 - {item.itemName}, 종목코드 - {item.itemCode}");
                    stockItemService.createItem(item);
                    goto End;
                }

                if (matchedItem.itemName != item.itemName) //종목코드는 같은데 종목명이 다른경우는 종목명을 업데이트 해 준다.
                {
                    Logger.Info($"종목명이 바뀐 종목: 등록된 이름 - {matchedItem.itemName}, 바뀐 이름 - {item.itemName}");
                    stockItemService.updateItemName(item);
                }

            End:;
                
            }

            Logger.Info("전체 종목리스트 업데이트 완료");

        }
    }
}
