using CloudRealtime.SevenBread.handler;
using CloudRealtime.SevenBread.model;
using CloudRealtime.SevenBread.service;
using CloudRealtime.util;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

namespace CloudRealtime.SevenBread.controller
{
    public partial class SevenBreadController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private static String kafkaAddress = ConfigurationManager.AppSettings.Get("KafkaAddress");
        private static String topicName = ConfigurationManager.AppSettings.Get("SevenBreadTopicName");
        private AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        private SevenBreadService sevenBreadService;
        private List<SevenBreadItem> sevenBreadItemList;
        private List<SevenBreadDeletedItem> sevenBreadDeletedItemList;
        private MyTelegramBot myTelegramBot;
        private Opt10001EventHandler opt10001EventHandler;
        private Opt10086EventHandler opt10086EventHandler;
        private Opt10081EventHandler opt10081EventHandler;
        private RealDataEventHandler realDataEventHandler;

        public SevenBreadController(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI1 = axKHOpenAPI;
            this.sevenBreadService = new SevenBreadService();
            this.myTelegramBot = new MyTelegramBot();
            this.opt10001EventHandler = new Opt10001EventHandler(axKHOpenAPI);
            this.opt10086EventHandler = new Opt10086EventHandler(axKHOpenAPI);
            this.opt10081EventHandler = new Opt10081EventHandler(axKHOpenAPI);

            initialize();
        }

        private void initialize()
        {
            //COMPLETE. 007빵에 새로운 종목이 등록되면 종목정보를 업데이트하기
            Thread t1 = new Thread(new ThreadStart(() =>
            {
                sevenBreadItemconsumer();
            }));

            t1.Start();

            //Thread t2 = new Thread(new ThreadStart(() =>
            //{
            //    Thread.Sleep(10000);
            //    //COMPLETE. 007빵 리스트 가져오기 구현해야 함
            //    this.sevenBreadItemList = this.sevenBreadService.getSevenBreadItemList();
            //    this.realDataEventHandler = new RealDataEventHandler(this.axKHOpenAPI1, this.sevenBreadItemList);

            //    Logger.Info("===== 007빵리스트 종목 등록 중...");
            //    try
            //    {
            //        if (this.sevenBreadItemList == null) throw new Exception("종목리스트 가져오기 실패");
            //        foreach (SevenBreadItem item in this.sevenBreadItemList)
            //        {
            //            Logger.Info($"{item.itemName}({item.itemCode}) 종목이 등록되었습니다. 기준가격 : {item.capturedPrice} 원");
            //            this.realDataEventHandler.setRealReg("7000", item.itemCode, "20;10;11;12;15;13;14;16;17;18;30", "1");
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Logger.Error(e.Message);
            //        myTelegramBot.sendTextMessageAsyncToSwingBot($"[007빵] 종목 등록 중 오류발생: {e.Message}");
            //    }
            //}));

            //t2.Start();

            Thread t3 = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(10000);
                //TimeSpan triggerTime = new TimeSpan(0, 0, 0);
                TimeSpan triggerTime = new TimeSpan(19, 35, 0);
                while (true)
                {
                    TimeSpan timeNow = DateTime.Now.TimeOfDay;

                    if (timeNow > triggerTime)
                    {
                        Logger.Info("007빵 종목 종가정보 업데이트 쓰레드 시작");
                        try
                        {
                            this.sevenBreadItemList = this.sevenBreadService.getSevenBreadItemList();

                            // opt10081 일봉데이터 업데이트
                            //foreach (SevenBreadItem item in this.sevenBreadItemList)
                            //{
                            //    //if (item.itemCode != "041510") continue;
                            //    opt10081EventHandler.requestTrOpt10081(item.itemCode, item.capturedDate, "0", $"007빵_일봉차트");
                            //    Thread.Sleep(20000);
                            //}

                            //Thread.Sleep(10000);

                            // opt10086 일별수급데이터 업데이트
                            foreach (SevenBreadItem item in this.sevenBreadItemList)
                            {
                                //if (item.itemCode != "041510") continue;
                                DateTime dt;
                                DateTime today = DateTime.Now;
                                string strDay = today.ToString("yyyy-MM-dd");
                                dt = Convert.ToDateTime(item.capturedDate);
                                int i = 0;
                                Logger.Debug($"{item.itemCode} 에 대한 수집 시작");
                                while (true)
                                {
                                    string strDt = dt.AddDays(i).ToString("yyyy-MM-dd");

                                    Logger.Debug($"{item.itemCode} 의 {strDt} 부터, {strDay} 까지");

                                    opt10086EventHandler.requestTrOpt10086(item.itemCode, strDt, $"007빵_일별수급");
                                    Thread.Sleep(3000);

                                    i++;
                                    if (strDt == strDay) break;
                                }
                                Thread.Sleep(10000);
                            }


                            //this.sevenBreadItemList = this.sevenBreadService.getSevenBreadItemList();
                            //this.sevenBreadDeletedItemList = this.sevenBreadService.getSevenBreadDeletedItemList();

                            //foreach (SevenBreadItem item in this.sevenBreadItemList)
                            //{
                            //    SevenBreadDeletedItem sevenBreadDeletedItem = new SevenBreadDeletedItem();
                            //    sevenBreadDeletedItem.itemName = item.itemName;
                            //    sevenBreadDeletedItem.itemCode = item.itemCode;
                            //    sevenBreadDeletedItem.capturedDate = item.capturedDate;
                            //    sevenBreadDeletedItem.capturedPrice = item.capturedPrice;

                            //    this.sevenBreadDeletedItemList.Add(sevenBreadDeletedItem);
                            //}

                            //this.sevenBreadDeletedItemList = this.sevenBreadDeletedItemList.Distinct().ToList();

                            //foreach (SevenBreadDeletedItem item in this.sevenBreadDeletedItemList)
                            //{
                            //    opt10001EventHandler.requestTrOpt10001(item.itemCode, $"007빵삭제종가업데이트_{item.itemName}_{item.capturedDate}_{item.capturedPrice}");
                            //    Thread.Sleep(1500);
                            //}

                            //DateTime today = DateTime.Now;
                            //string strNow = today.ToString("yyyy년 MM월 dd일");
                            //string strDay = today.ToString("yyyy-MM-dd");

                            //myTelegramBot.sendTextMessageAsyncToSwingBot($"✔️ {strNow} 007빵 종가/통계 업데이트가 완료되었습니다.");
                            break;
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e.Message);
                        }
                    }
                }
            }));

            t3.Start();

            //Thread t4 = new Thread(new ThreadStart(() =>
            //{
            //    Thread.Sleep(10000);
            //    Logger.Info("007빵 삭제종목 종가정보 업데이트 쓰레드 시작");
            //    TimeSpan triggerTime = new TimeSpan(15, 50, 0);
            //    while (true)
            //    {
            //        TimeSpan timeNow = DateTime.Now.TimeOfDay;

            //        if (timeNow > triggerTime)
            //        {
            //            try
            //            {
            //                this.sevenBreadDeletedItemList = this.sevenBreadService.getSevenBreadDeletedItemList();
            //                foreach (SevenBreadDeletedItem item in this.sevenBreadDeletedItemList)
            //                {
            //                    opt10001EventHandler.requestTrOpt10001(item.itemCode, $"007빵삭제종가업데이트_{item.itemName}_{item.capturedDate}_{item.capturedPrice}");
            //                    Thread.Sleep(1500);
            //                }

            //                DateTime today = DateTime.Now;
            //                string strNow = today.ToString("yyyy년 MM월 dd일");
            //                string strDay = today.ToString("yyyy-MM-dd");

            //                myTelegramBot.sendTextMessageAsyncToBot($"✔️ {strNow} 007빵 통계 업데이트가 완료되었습니다.");
            //                break;
            //            }
            //            catch (Exception e)
            //            {
            //                Logger.Error(e.Message);
            //            }
            //        }
            //    }
            //}));

            //t4.Start();
        }



        private void sevenBreadItemconsumer()
        {
            // https://eecs.blog/c-sending-and-receiving-data-from-a-thread/ 이거 참고해서 만들 것
            var conf = new ConsumerConfig
            {
                GroupId = "sevenBread-consumer-group",
                //BootstrapServers = "myhency.duckdns.org:29093",
                BootstrapServers = kafkaAddress,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(topicName);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            // 카프카에 새로 등록된 007빵 종목
                            SevenBreadItemCreationRequestDto sevenBreadItem = JsonConvert.DeserializeObject<SevenBreadItemCreationRequestDto>(cr.Message.Value);
                            
                            Logger.Info($"[007빵] 새롭게 추가된 종목 : {sevenBreadItem.itemName}[{cr.Message.Value}]");
                            myTelegramBot.sendTextMessageAsyncToSwingBot($"[007빵] 새롭게 추가된 종목 : {sevenBreadItem.itemName}[{sevenBreadItem.itemCode}]");
                            opt10086EventHandler.requestTrOpt10086(sevenBreadItem.itemCode, sevenBreadItem.capturedDate, $"007빵_일별주가");
                            Thread.Sleep(1000);
                            //opt10001EventHandler.requestTrOpt10001(sevenBreadItem.itemCode, $"뉴007빵종가업데이트");
                            //Thread.Sleep(1000);
                            //opt10081EventHandler.requestTrOpt10081(sevenBreadItem.itemCode, sevenBreadItem.capturedDate, "0", $"007빵_일봉차트");
                            //Thread.Sleep(1000);

                        }
                        catch (ConsumeException e)
                        {
                            Logger.Error($"[007빵] Kafka consumer 오류:{e.Error.Reason}");
                            myTelegramBot.sendTextMessageAsyncToSwingBot($"[007빵] Kafka consumer 오류:{e.Error.Reason}");
                        }
                        catch (JsonSerializationException e)
                        {
                            Logger.Error($"[007빵] json convert 오류:{e.Message}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
        }
    }
}