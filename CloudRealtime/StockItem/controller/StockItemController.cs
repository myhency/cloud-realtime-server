using CloudRealtime.controllers;
using CloudRealtime.StockItem.model;
using CloudRealtime.StockItem.service;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CloudRealtime.StockItem.controller
{
    public partial class StockItemController : CloudController
    {
        private OtherFunctions otherFunctions;
        private StockItemService stockItemService;

        public StockItemController(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI) : base(axKHOpenAPI)
        {
            this.otherFunctions = new OtherFunctions(base.axKHOpenAPI1);
            this.stockItemService = new StockItemService();

            initialize();
        }

        private void initialize()
        {
            List<Item> serviceItemList = stockItemService.getRegisterItems();
            List<Item> marketItemList = GetItemListBy(otherFunctions.GetCodeList("All"));

            logger.Debug($"종목수는 총 {marketItemList.Count}개 입니다");

            // 신규등록된 종목에 대한 처리
            GetMatchedItems(marketItemList, serviceItemList, isMatched: false)
                .ForEach(item =>
                {
<<<<<<< HEAD
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
                        || item.itemName.Contains("SOL ")
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
                        || item.itemName.Contains("(합성)")
                        || (item.itemName.Contains("스팩") && item.itemName.EndsWith("호"))
                    ) //ETN, ETF, 스팩 제외
                    {
                        goto End;
                    }

                    Logger.Info($"등록되지 않은 종목: 종목명 - {item.itemName}, 종목코드 - {item.itemCode}");
                    myTelegramBot.sendTextMessageAsyncToSwingBot($"✔📰 신규상장 : 종목명 - {item.itemName}, 종목코드 - {item.itemCode}");
=======
                    logger.Info($"등록되지 않은 종목: 종목명 - {item.itemName}, 종목코드 - {item.itemCode}");
                    base.myTelegramBot.sendTextMessageAsyncToBot($"✔📰 신규상장 : 종목명 - {item.itemName}, 종목코드 - {item.itemCode}");
>>>>>>> server-change
                    stockItemService.createItem(item);
                    Thread.Sleep(500);
                });

            // 이름이 변경된 종목에 대한 처리
            GetMatchedItems(marketItemList, serviceItemList)
                    .Where(matchedItemObj =>
                        serviceItemList.All(serviceItemObj =>
                            !serviceItemObj.itemName.Equals(matchedItemObj.itemName)))
                    .ToList()
                    .ForEach(item =>
                    {
                        logger.Info(
                            $"종목명이 바뀐 종목: 등록된 이름 - " +
                            $"{serviceItemList.Find(serviceItemObj => serviceItemObj.itemCode.Equals(item.itemCode)).itemName}" +
                            $", 바뀐 이름 - {item.itemName}"
                        );
                        base.myTelegramBot.sendTextMessageAsyncToBot(
                            $"✔📰 종목명이 변경됨 : 원래 이름 - " +
                            $"{serviceItemList.Find(serviceItemObj => serviceItemObj.itemCode.Equals(item.itemCode)).itemName}, " +
                            $"바뀐 이름 - {item.itemName}"
                        );
                        stockItemService.updateItemName(item);
                        Thread.Sleep(500);
                    });

            //foreach (Item item in marketItemList)
            //{
            //    Item matchedItem = serviceItemList.FirstOrDefault(v => v.itemCode == item.itemCode);

            //    if (matchedItem is null)
            //    {
            //        if (IsExcluded(item))
            //        {
            //            goto End;
            //        }

            //        logger.Info($"등록되지 않은 종목: 종목명 - {item.itemName}, 종목코드 - {item.itemCode}");
            //        base.myTelegramBot.sendTextMessageAsyncToBot($"✔📰 신규상장 : 종목명 - {item.itemName}, 종목코드 - {item.itemCode}");
            //        stockItemService.createItem(item);
            //        Thread.Sleep(500);
            //        goto End;
            //    }

            //    if (matchedItem.itemName != item.itemName) //종목코드는 같은데 종목명이 다른경우는 종목명을 업데이트 해 준다.
            //    {
            //        logger.Info($"종목명이 바뀐 종목: 등록된 이름 - {matchedItem.itemName}, 바뀐 이름 - {item.itemName}");
            //        base.myTelegramBot.sendTextMessageAsyncToBot($"✔📰 종목명이 변경됨 : 원래 이름 - {matchedItem.itemName}, 바뀐 이름 - {item.itemName}");
            //        stockItemService.updateItemName(item);
            //        Thread.Sleep(500);
            //    }

            //End:;
            //}

            logger.Info("[StockItems] 전체 종목리스트 업데이트 완료");
            base.myTelegramBot.sendTextMessageAsyncToSwingBot("[StockItems] 전체 종목리스트 업데이트 완료");
        }

        private List<Item> GetMatchedItems(List<Item> marketItemList, List<Item> serviceItemList, bool isMatched = true)
        {
            if (isMatched)
                return marketItemList.Where(marketObj =>
                    serviceItemList.Any(serviceObj =>
                        serviceObj.itemCode == marketObj.itemCode))
                    .ToList()
                    .FindAll(item => !IsExcluded(item));

            return marketItemList.Where(marketObj =>
                    !serviceItemList.Any(serviceObj =>
                        serviceObj.itemCode == marketObj.itemCode))
                .ToList()
                .FindAll(item => !IsExcluded(item));
        }

        private List<Item> GetItemListBy(List<string> codeList)
        {
            return codeList.Select(code =>
                new Item
                {
<<<<<<< HEAD
                    Logger.Info($"종목명이 바뀐 종목: 등록된 이름 - {matchedItem.itemName}, 바뀐 이름 - {item.itemName}");
                    myTelegramBot.sendTextMessageAsyncToSwingBot($"✔📰 종목명이 변경됨 : 원래 이름 - {matchedItem.itemName}, 바뀐 이름 - {item.itemName}");
                    stockItemService.updateItemName(item);
                    Thread.Sleep(500);
                }
=======
                    itemCode = code,
                    itemName = otherFunctions.GetMasterCodeName(code)
                })
                .ToList();
        }
>>>>>>> server-change

        private bool IsExcluded(Item item)
        {
            if (item.itemName.Contains(" ETN")
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
                || item.itemName.Contains("SOL ")
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
                || item.itemName.Contains("(합성)")
                || (item.itemName.Contains("스팩") && item.itemName.EndsWith("호")))
            {
                return true;
            }

            return false;
        }
    }
}
